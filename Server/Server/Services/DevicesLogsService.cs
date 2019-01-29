﻿using DataProviderCommon;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Server.Helpers;
using Server.Models;
using Server.Repository;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public class DevicesLogsService : IDevicesLogsService
    {
        private readonly DeviceHelperType _devicePluginsHelper;
        private readonly UserSettings _userSettings;
        private readonly DataStoragesHelperType _dataStoragesHelper;
        private readonly IDevicesLogsRepository _devicesLogsRepository;

        public DevicesLogsService(DeviceHelperType devicePluginsHelper
            , DataStoragesHelperType dataStoragesHelper
            , AppSettingsAccessor appSettingsModifier
            , IDevicesLogsRepository devicesLogsRepository
            )
        {
            _userSettings = appSettingsModifier.GetServerSettings();
            _devicePluginsHelper = devicePluginsHelper;
            _dataStoragesHelper = dataStoragesHelper;
            _devicesLogsRepository = devicesLogsRepository;
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

            Random random = new Random();
            deviceLog.DateStamp = deviceLog.DateStamp.AddHours(random.Next(1,7));

            return deviceLog;
        }

        public IEnumerable<DataStoragePluginViewModel> GetDataStoragePlugins()
        {
            return _dataStoragesHelper.GetDataStoragePluginNames();
        }

        public Task<List<DeviceLog>> GetDeviceLogsAsync(int? utcDate)
        {
            return _devicesLogsRepository.GetDeviceLogsAsync(utcDate);
        }

        public IEnumerable<DevicePluginViewModel> GetDevicePlugins()
        {
            return _devicePluginsHelper.GetDeviceluginNames();
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
            if (!logs.Any()) {
                return null;
            }
            var group = logs.GroupBy(l => l.PluginName).FirstOrDefault(gr => gr.Key == deviceName);

            IDevicePlugin plugin = _devicePluginsHelper.GetDevicePlugin(group.Key);
            var dataForUI = group.Select(log => log).ToList();

            return plugin.PrepareDataForUI(dataForUI);
        }
        
    }
}
