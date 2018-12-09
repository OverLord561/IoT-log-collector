using DataProviderCommon;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Server.Extensions;
using Server.Repository;
using Server.Services;
using Server.SynchronyHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api/log-collector")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDataStoragePlugin _dataStoragePlugin;
        private readonly DeviceHelperType _devicePluginsHelper;
        private readonly SynchronyHelper _synchronyHelper;
        private readonly IDevicesLogsRepository _deviceLogsRepository;
        private readonly IDevicesLogsService _devicesLogsService;


        public DeviceController(IConfiguration configuration,
            DataStoragesHelperType dataStoragesHelper,
            DeviceHelperType devicePluginsHelper,
            SynchronyHelper synchronyHelper,
            IDevicesLogsRepository deviceLogsRepository,
            IDevicesLogsService devicesLogsService
            )
        {
            _dataStoragePlugin = dataStoragesHelper.GetDataStoragePlugin() ?? throw new ArgumentNullException(nameof(dataStoragesHelper));
            _devicePluginsHelper = devicePluginsHelper;
            _synchronyHelper = synchronyHelper;
            _deviceLogsRepository = deviceLogsRepository;
            _devicesLogsService = devicesLogsService;
        }

        [HttpPost]
        [Route("write-log")]
        public async Task<IActionResult> WriteLog([FromBody] string smthFromDevice)
        {
            for (var i = 0; i < 10; i++)
            {
                var generalPluginInfo = JsonConvert.DeserializeObject<StandardizedMessageFromDevice>(smthFromDevice);

                var plugin = _devicePluginsHelper.GetDevicePlugin(generalPluginInfo.PluginName);/* SamsungDPlugin*/

                if (plugin == null)
                {
                    throw new ArgumentNullException(nameof(plugin));
                }

                var standardizedDevice = plugin.ConverterToStandard(smthFromDevice);

                standardizedDevice.DateStamp = standardizedDevice.DateStamp.AddHours(i);

                await _dataStoragePlugin.Operations.AddAsync(standardizedDevice);

                _synchronyHelper.UpdateCounter();

            }

            return Ok("success");
        }

        [HttpGet]
        [Route("get-logs")]
        [EnableCors("AllowSpecificOrigin")]
        public IActionResult GetLogs(int? utcDate)
        {
            _synchronyHelper.EventSlim.Wait();

            var logs = _deviceLogsRepository.GetDeviceLogs(utcDate);
            var logsForUI = _devicesLogsService.PrepareLogsForUI(logs);           

            _synchronyHelper.EventSlim.Reset();

            return new JsonResult(new { StatusCode = StatusCodes.Status200OK, Logs = logsForUI });
        }

    }
}