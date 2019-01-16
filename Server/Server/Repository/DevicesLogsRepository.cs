using DataProviderCommon;
using Server.Extensions;
using Server.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Repository
{
    public class DevicesLogsRepository : IDevicesLogsRepository
    {
        private readonly IDataStoragePlugin _dataStoragePlugin;
        private readonly CollectionOfLogs _collectionOfLogs;

        public DevicesLogsRepository(DataStoragesHelperType dataStoragesHelper, CollectionOfLogs collectionOfLogs)
        {
            _dataStoragePlugin = dataStoragesHelper.GetDataStoragePlugin() ?? throw new ArgumentNullException(nameof(dataStoragesHelper));
            _collectionOfLogs = collectionOfLogs;
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

        public bool WriteLogToTemporaryCollection(DeviceLog log)
        {
            return _collectionOfLogs.AddLog(log);
        }

        public async Task<bool> WriteRangeAsync(List<DeviceLog> logs)
        {
            return await _dataStoragePlugin.Operations.AddRangeAsync(logs);
        }
    }
}
