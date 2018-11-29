using DataProviderCommon;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;

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
            var generalPluginInfo = JsonConvert.DeserializeObject<StandardizedMessageFromDevice>(smthFromDevice);

            var plugin = _devicePluginsHelper.GetDevicePlugin(generalPluginInfo.PluginName);/* SamsungDPlugin*/

            if (plugin == null)
            {
                throw new ArgumentNullException(nameof(plugin));
            }

            var standardizedDevice = plugin.ConverterToStandard(smthFromDevice);

            _dataStoragePlugin.Operations.Add(standardizedDevice);

            return Ok("success");
        }
    }
}