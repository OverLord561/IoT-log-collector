using DataProviderCommon;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Helpers;
using Server.Repository;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api/log-collector")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly DBWriterHelper _synchronyHelper;
        private readonly IDevicesLogsRepository _deviceLogsRepository;
        private readonly IDevicesLogsService _devicesLogsService;

        static int count;
        static int count2;

        static object countLock = new object();

        static DeviceController()
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
                    //System.Diagnostics.Debugger.Log(0, "", $"{cnt}/s \r\n");
                    Console.WriteLine($"{cnt}/s \r\n");

                }
            });
        }

        public DeviceController(
            DBWriterHelper synchronyHelper,
            IDevicesLogsRepository deviceLogsRepository,
            IDevicesLogsService devicesLogsService
            )
        {
            _synchronyHelper = synchronyHelper;
            _deviceLogsRepository = deviceLogsRepository;
            _devicesLogsService = devicesLogsService;
        }

        [HttpPost]
        [Route("write-log")]
        public async Task<IActionResult> WriteLog( string smthFromDevice)
        {

            smthFromDevice = "{\"PluginName\":\"SamsungDPlugin\",\"DeviceData\":{\"Temperature\":10.0,\"Humidity\":10.0}}";
            lock (countLock)
            {
                count++;
                count2++;
            }

            try
            {
                await _deviceLogsRepository.WriteLog(smthFromDevice, count2);
            }
            catch (Exception ex)
            {
                var errors = ex;
            }

            return Ok("success");
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
                    //_synchronyHelper.EventSlim.Wait();

                    var logs = await _deviceLogsRepository.GetDeviceLogsAsync(utcDate);
                    logsForUI = _devicesLogsService.PrepareLogsForUI(logs);

                    //_synchronyHelper.EventSlim.Reset();
                }
                else
                {
                    var logs = await _deviceLogsRepository.GetDeviceLogsAsync(utcDate);
                    logsForUI = _devicesLogsService.PrepareLogsForUI(logs);
                }

                return new JsonResult(new { StatusCode = StatusCodes.Status200OK, Logs = logsForUI });
            }
            catch (Exception ex)
            {
                var error = ex;

                return Ok("success");
            }
        }
    }
}