using DataProviderCommon;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Helpers;
using Server.Repository;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api/log-collector")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly CollectionOfLogs _collectionOfLogs;
        private readonly IDevicesLogsRepository _deviceLogsRepository;
        private readonly IDevicesLogsService _devicesLogsService;

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

        public DeviceController(
            CollectionOfLogs collectionOfLogs,
            IDevicesLogsRepository deviceLogsRepository,
            IDevicesLogsService devicesLogsService
            )
        {
            _collectionOfLogs = collectionOfLogs;
            _deviceLogsRepository = deviceLogsRepository;
            _devicesLogsService = devicesLogsService;
        }

        [HttpPost]
        [Route("write-log")]
        public IActionResult WriteLog(string smthFromDevice)
        {
            smthFromDevice = "{\"PluginName\":\"SamsungDPlugin\",\"DeviceData\":{\"Temperature\":10.0,\"Humidity\":10.0}}";
            lock (countLock)
            {
                count++;
            }

            try
            {
                var log = _devicesLogsService.ConvertStringToDeviceLog(smthFromDevice);

                _deviceLogsRepository.WriteLogToTemporaryCollection(log);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Ok("Log added to temporary collection");
        }

        [HttpGet]
        [Route("get-logs")]
        [EnableCors("AllowSPAAccess")]
        public async Task<IActionResult> GetLogs(int? utcDate, bool isInitial)
        {
            try
            {
                var logsForUI = new List<IDeviceLogsUIFormat>();

                if (!isInitial)
                {
                    _collectionOfLogs.resetEvent.WaitOne();

                    _collectionOfLogs.resetEvent.Reset();
                }

                var logs = await _deviceLogsRepository.GetDeviceLogsAsync(utcDate);
                logsForUI = _devicesLogsService.PrepareLogsForUI(logs);

                return new JsonResult(new { StatusCode = StatusCodes.Status200OK, Logs = logsForUI });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return BadRequest(ex.Message);
            }
        }
    }
}