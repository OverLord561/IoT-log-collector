using DataProviderCommon;
using Server.ViewModels;
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

        public IEnumerable<DevicePluginViewModel> GetDeviceluginNames()
        {
            return _devicePluginsCollection.Select(x =>
                new DevicePluginViewModel
                {
                    DisplayName = x.DisplayName,
                    Value = x.PluginName,
                }
            );
        }
    }
}
