using DataProviderCommon;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Server.Helpers;
using Server.Models;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Server.Services
{
    public class DevicesLogsService : IDevicesLogsService
    {
        private readonly DeviceHelperType _devicePluginsHelper;
        private readonly UserSettings _userSettings;
        private readonly DataStoragesHelperType _dataStoragesHelper;

        public DevicesLogsService(DeviceHelperType devicePluginsHelper, IOptionsSnapshot<UserSettings> subOptionsAccessor
            , DataStoragesHelperType dataStoragesHelper)
        {
            _userSettings = subOptionsAccessor.Value;
            _devicePluginsHelper = devicePluginsHelper;
            _dataStoragesHelper = dataStoragesHelper;
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

            return deviceLog;
        }

        public IEnumerable<DataStoragePluginViewModel> GetDataStoragePlugins()
        {
            return _dataStoragesHelper.GetDataStoragePluginNames();
        }

        public IEnumerable<ServerSettingViewModel> GetServerSettings()
        {
            return new List<ServerSettingViewModel>() {
                new ServerSettingViewModel()
                {
                    Name = "DataStoragePlugin",
                    Value = _userSettings.DataProviderPluginName,
                    DisplayName = "Data storage plugin",
                    IsEditable = false
                },
                new ServerSettingViewModel()
                {
                    Name = "BulkInsertCapacity",
                    DisplayName = "Bulk insert capacity",
                    Value = _userSettings.CapacityOfCollectionToInsert.ToString(),
                    IsEditable = true
                },
                new ServerSettingViewModel()
                {
                    Name = "BulkInsertInterval",
                    DisplayName = "Bulk insert interval",
                    Value = _userSettings.IntervalForWritingIntoDb.ToString(),
                    IsEditable = true
                }
            };
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
