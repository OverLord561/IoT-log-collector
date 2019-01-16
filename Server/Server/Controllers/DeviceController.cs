﻿using DataProviderCommon;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Helpers;
using Server.Repository;
using Server.Services;
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

        private static void IncrementCount()
        {
            lock (countLock)
            {
                count++;
            }
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
        public async Task<IActionResult> WriteLogAsync(string smthFromDevice)
        {
            // TODO handle application/json request
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8)) // sdsd
            {
                smthFromDevice = await reader.ReadToEndAsync();
            }


            IncrementCount();


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
        public async Task<IActionResult> GetLogs(int? utcDate, bool isInitial, string deviceName = "SamsungDPlugin")
        {
            try
            {
                if (!isInitial)
                {
                    _collectionOfLogs.resetEvent.WaitOne();
                }

                var logs = await _deviceLogsRepository.GetDeviceLogsAsync(utcDate);
                DeviceLogsInChartFormat logsForUI = _devicesLogsService.PrepareLogsForUI(logs, deviceName);

                return new JsonResult(new { StatusCode = StatusCodes.Status200OK, ChartData = logsForUI });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return BadRequest(ex.Message);
            }
        }
    }
}