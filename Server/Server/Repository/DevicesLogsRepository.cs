using DataProviderCommon;
using Server.Extensions;
using Server.Helpers;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Server.Repository
{
    public class DevicesLogsRepository : IDevicesLogsRepository
    {
        private IDataStoragePlugin _dataStoragePlugin;
        private readonly CollectionOfLogs _collectionOfLogs;
        private readonly DataStoragesHelperType _dataStoragesHelperType;


        public DevicesLogsRepository(DataStoragesHelperType dataStoragesHelper, CollectionOfLogs collectionOfLogs, AppSettingsAccessor appSettingsAccessor)
        {
            _dataStoragePlugin = dataStoragesHelper.GetDataStoragePlugin() ?? throw new ArgumentNullException(nameof(dataStoragesHelper));
            _collectionOfLogs = collectionOfLogs;

            _dataStoragesHelperType = dataStoragesHelper;
            //appSettingsAccessor.NotifyDependentEntetiesEvent += HandleUserSettingsUpdate;
        }

        public async Task<List<DeviceLog>> GetDeviceLogsAsync(int? utcDate)
        {
            if (utcDate == null)
            {
                return await _dataStoragePlugin.Operations.AllAsync();
            }
            else
            {
                DateTime _date = utcDate.Value.FromUtcToLocalTime();
                return _dataStoragePlugin.Operations.Get(d => d.DateStamp.Day == _date.Day);
            }
        }

        public async Task<bool> WriteRangeAsync(List<DeviceLog> logs)
        {
            _dataStoragePlugin = _dataStoragesHelperType.GetDataStoragePlugin();
            try
            {
                await _dataStoragePlugin.Operations.AddRangeAsync(logs);
            }
            catch (Exception ex)
            {
                Debugger.Break();
            }

            return true;
        }

        public bool WriteRange(List<DeviceLog> logs)
        {
            _dataStoragePlugin = _dataStoragesHelperType.GetDataStoragePlugin();
            try
            {
                _dataStoragePlugin.Operations.AddRange(logs);
            }
            catch (Exception ex)
            {
                Debugger.Break();
            }

            return true;
        }
    }
}
