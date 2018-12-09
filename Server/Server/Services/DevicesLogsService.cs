using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataProviderCommon;

namespace Server.Services
{
    public class DevicesLogsService : IDevicesLogsService
    {
        private readonly DeviceHelperType _devicePluginsHelper;
        public DevicesLogsService(DeviceHelperType devicePluginsHelper)
        {
            _devicePluginsHelper = devicePluginsHelper;
        }
        public List<IDeviceLogsUIFormat> PrepareLogsForUI(List<DeviceLog> logs)
        {
            var groups = logs.GroupBy(l => l.PluginName);

            List<IDeviceLogsUIFormat> devicesLogs = new List<IDeviceLogsUIFormat>();

            foreach (var group in groups)
            {
                IDevicePlugin plugin = _devicePluginsHelper.GetDevicePlugin(group.Key);
                var dataForUI = group.Select(log => log).ToList();

                devicesLogs.Add(plugin.PrepareDataForUI(dataForUI));
                devicesLogs.Add(plugin.PrepareDataForUI(dataForUI));
            }

            return devicesLogs;
        }
    }
}
