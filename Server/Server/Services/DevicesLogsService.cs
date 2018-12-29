using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataProviderCommon;
using Newtonsoft.Json;
using Server.Helpers;

namespace Server.Services
{
    public class DevicesLogsService : IDevicesLogsService
    {
        private readonly DeviceHelperType _devicePluginsHelper;
        public DevicesLogsService(DeviceHelperType devicePluginsHelper)
        {
            _devicePluginsHelper = devicePluginsHelper;
        }

        public DeviceLog ConvertStringToDeviceLog(string messageFromDevice)
        {
            var generalPluginInfo = JsonConvert.DeserializeObject<StandardizedMessageFromDevice>(messageFromDevice);

            var plugin = _devicePluginsHelper.GetDevicePlugin(generalPluginInfo.PluginName); /* SamsungDPlugin*/

            if (plugin == null)
            {
                throw new ArgumentNullException(nameof(plugin));
            }

            var deviceLog = plugin.ConverterToStandard(messageFromDevice);
            deviceLog.DateStamp = deviceLog.DateStamp.AddHours(0);
            deviceLog.PluginName = string.Concat(deviceLog.PluginName);

            return deviceLog;
        }

        public DeviceLogsInChartFormat PrepareLogsForUI(List<DeviceLog> logs, string deviceName)
        {
            var group = logs.GroupBy(l => l.PluginName).FirstOrDefault(gr => gr.Key == deviceName);

            IDevicePlugin plugin = _devicePluginsHelper.GetDevicePlugin(group.Key);
            var dataForUI = group.Select(log => log).ToList();

            return plugin.PrepareDataForUI(dataForUI);
        }
    }
}
