using DataProviderCommon;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Server.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Controllers
{
    [Route("api/log-collector")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDataStoragePlugin _dataStoragePlugin;
        private readonly DeviceHelperType _devicePluginsHelper;

        public DeviceController(IConfiguration configuration,
            DataStoragesHelperType dataStoragesHelper,
            DeviceHelperType devicePluginsHelper
            )
        {
            _dataStoragePlugin = dataStoragesHelper.GetDataStoragePlugin() ?? throw new ArgumentNullException(nameof(dataStoragesHelper));
            _devicePluginsHelper = devicePluginsHelper;
        }

        [HttpPost]
        [Route("write-log")]
        public IActionResult WriteLog([FromBody] string smthFromDevice)
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
                _dataStoragePlugin.Operations.Add(standardizedDevice);

            }

            return Ok("success");
        }

        [HttpGet]
        [Route("get-logs")]
        public IActionResult GetLogs(int? utcDate)
        {
            var logs = new List<DeviceLog>();

            if (utcDate == null)
            {
                logs = _dataStoragePlugin.Operations.All();
            }
            else
            {
                DateTime _date = utcDate.Value.FromUtcToLocalTime();
                logs = _dataStoragePlugin.Operations.Get(d => d.DateStamp.Day == _date.Day);
            }

            var groups = logs.GroupBy(l => l.PluginName);

            List<IDeviceLogsUIFormat> devicesLogs = new List<IDeviceLogsUIFormat>();

            foreach (var group in groups)
            {
                IDevicePlugin plugin = _devicePluginsHelper.GetDevicePlugin(group.Key);
                var dataForUI = group.Select(log => log).ToList();

                devicesLogs.Add(plugin.PrepareDataForUI(dataForUI));
            }

            return new JsonResult(new { StatusCode = StatusCodes.Status200OK, Logs = devicesLogs });

        }

    }
}