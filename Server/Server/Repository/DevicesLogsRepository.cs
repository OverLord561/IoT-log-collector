using DataProviderCommon;
using Server.Extensions;
using Server.Helpers;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Repository
{
    public class DevicesLogsRepository : IDevicesLogsRepository
    {
        private IDataStoragePlugin _dataStoragePlugin;
        private readonly CollectionOfLogs _collectionOfLogs;
        private readonly static object _locker = new object();
        private readonly DataStoragesHelperType _dataStoragesHelperType;
        private readonly AppSettingsAccessor _appSettingsAccessor;


        public DevicesLogsRepository(DataStoragesHelperType dataStoragesHelper, CollectionOfLogs collectionOfLogs, AppSettingsAccessor appSettingsAccessor)
        {
            _dataStoragePlugin = dataStoragesHelper.GetDataStoragePlugin() ?? throw new ArgumentNullException(nameof(dataStoragesHelper));
            _collectionOfLogs = collectionOfLogs;

            _dataStoragesHelperType = dataStoragesHelper;
            _appSettingsAccessor = appSettingsAccessor;
            appSettingsAccessor.NotifyDependentEntetiesEvent += HandleUserSettingsUpdate;
        }

        private void HandleUserSettingsUpdate()
        {
            _dataStoragePlugin = _dataStoragesHelperType.GetDataStoragePlugin();
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
            lock (_locker)
            {
                return _dataStoragePlugin.Operations.AddRange(logs);
            }

        }
    }
}
