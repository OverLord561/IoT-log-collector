using DataProviderCommon;
using System.Collections.Generic;
using System.Linq;

namespace Server.Helpers
{
    public class DeviceHelperType
    {
        private readonly IEnumerable<IDevicePlugin> _devicePluginsCollection;

        public DeviceHelperType(IEnumerable<IDevicePlugin> devicePluginsCollection)
        {
            _devicePluginsCollection = devicePluginsCollection;
        }

        public IDevicePlugin GetDevicePlugin(string pluginName)
        {
            return _devicePluginsCollection.FirstOrDefault(x => x.PluginName == pluginName);
        }
    }
}
