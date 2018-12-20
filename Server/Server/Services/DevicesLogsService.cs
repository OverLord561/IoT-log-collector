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
            deviceLog.DateStamp = deviceLog.DateStamp.AddHours(-3);
            deviceLog.PluginName = string.Concat(deviceLog.PluginName);

            return deviceLog;
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
            }

            return devicesLogs;
        }
    }
}
