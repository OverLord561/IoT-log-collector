using DataProviderCommon;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api/log-collector")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDataStoragePlugin _dataStoragePlugin;
        private readonly IDevicePlugin _standardizedDevicePlugin;
        private readonly IEnumerable<IDevicePlugin> _devicePluginsCollection;


        public DeviceController(IConfiguration configuration, DataStoragesHelperType dataStorages,
            DeviceHelperType devicePlugins,
            IHttpContextAccessor httpContextAccessor,
            IEnumerable<IDevicePlugin> devicePluginsCollection
            )
        {
            _dataStoragePlugin = dataStorages.GetDataStoragePlugin() ?? throw new ArgumentNullException(nameof(dataStorages));
            _standardizedDevicePlugin = devicePlugins.GetDevicePlugin() ?? throw new ArgumentNullException(nameof(devicePlugins));
            _devicePluginsCollection = devicePluginsCollection;

        }

        [HttpPost]
        [Route("write-log")]
        public IActionResult WriteLog([FromBody] string smthFromDevice)
        {
            var generalPluginInfo = JsonConvert.DeserializeObject<StandardizedMessage>(smthFromDevice);

            var plugin = _devicePluginsCollection.FirstOrDefault(x => x.GetType().Name == generalPluginInfo.PluginType);/* SamsungTemperatureControllerPlugin*/

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