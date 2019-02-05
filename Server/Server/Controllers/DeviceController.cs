using DataProviderCommon;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Helpers;
using Server.Services;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api/log-collector")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly CollectionOfLogs _collectionOfLogs;
        private readonly IDevicesLogsService _devicesLogsService;
        private readonly AppSettingsAccessor _appSettingsModifier;

        static int count;
        private readonly static object countLock = new object();

        static DeviceController()
        {
            LogCountOFRequestPerSecond();
        }

        private static void LogCountOFRequestPerSecond()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);
                    int cnt;
                    lock (countLock)
                    {
                        cnt = count;
                        count = 0;
                    }

                    Console.WriteLine($"{cnt}/s \r\n");
                }
            });
        }

        private static void IncrementCount()
        {
            lock (countLock)
            {
                count++;
            }
        }

        public DeviceController(
            CollectionOfLogs collectionOfLogs
            , IDevicesLogsService devicesLogsService
            , AppSettingsAccessor appSettingsModifier
            )
        {
            _collectionOfLogs = collectionOfLogs;
            _devicesLogsService = devicesLogsService;
            _appSettingsModifier = appSettingsModifier;
        }

        [HttpPost]
        [Route("write-log")]
        public async Task<IActionResult> WriteLogAsync(string smthFromDevice)
        {            
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8)) 
            {
                smthFromDevice = await reader.ReadToEndAsync();
            }

            IncrementCount();

            try
            {
                var log = _devicesLogsService.ConvertStringToDeviceLog(smthFromDevice);

                _collectionOfLogs.AddLog(log);

            }
            catch (ArgumentNullException ex)
            {
                //Debugger.Break();
                Console.WriteLine(ex.Message);
            }

            return Ok("Log added to temporary collection");
        }

        [HttpGet]
        [Route("get-logs")]
        [EnableCors("AllowSPAAccess")]
        public async Task<IActionResult> GetLogs(int? utcDate, bool isInitial, string deviceName = "SamsungDPlugin")
        {
            try
            {
                if (!isInitial)
                {
                    _collectionOfLogs.resetEvent.WaitOne();
                }

                var logs = await _devicesLogsService.GetDeviceLogsAsync(utcDate);
                DeviceLogsInChartFormat logsForUI = _devicesLogsService.PrepareLogsForUI(logs, deviceName);

                return new JsonResult(new { StatusCode = StatusCodes.Status200OK, ChartData = logsForUI });
            }
            catch (Exception ex)
            {
                //Debugger.Break();

                Console.WriteLine(ex.Message);

                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-sever-settings")]
        [EnableCors("AllowSPAAccess")]
        public IActionResult GetServerSettings()
        {
            IEnumerable<ServerSettingViewModel> serverSettings = _devicesLogsService.GetServerSettings();

            return new JsonResult(new { StatusCode = StatusCodes.Status200OK, ServerSettings = serverSettings });
        }


        [HttpGet]
        [Route("get-datastorageplugins-settings")]
        [EnableCors("AllowSPAAccess")]
        public IActionResult GetDataStoragePlugins()
        {
            var plugins = _devicesLogsService.GetDataStoragePluginsNames();

            return new JsonResult(new { StatusCode = StatusCodes.Status200OK, DataStoragePlugins = plugins });
        }

        [HttpPut]
        [Route("update-datastorageplugins-settings")]
        [EnableCors("AllowSPAAccess")]
        public IActionResult UpdateCurrentDataStoragePlugin([FromBody] DataStoragePluginViewModel dataStoragePlugin)
        {
            bool succeeded = _appSettingsModifier.UpdateDataStoragePlugin(dataStoragePlugin);

            return new JsonResult(new { StatusCode = StatusCodes.Status200OK, Succeeded = succeeded });
        }

        [HttpPut]
        [Route("update-sever-settings")]
        [EnableCors("AllowSPAAccess")]
        public IActionResult SetServerSettings([FromBody] List<ServerSettingViewModel> serverSettings)
        {
            try
            {
                bool succeeded = _appSettingsModifier.UpdateServerSettings(serverSettings);

                return new JsonResult(new { StatusCode = StatusCodes.Status200OK, Succeeded = succeeded });

            }
            catch (Exception ex)
            {
                //Debugger.Break();
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-deviceplugins")]
        [EnableCors("AllowSPAAccess")]
        public IActionResult GetDevicePlugins()
        {
            var devicePlugins = _devicesLogsService.GetDevicePlugins();

            return new JsonResult(new { StatusCode = StatusCodes.Status200OK, DevicePlugins = devicePlugins });
        }
    }
}