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
        private IDataStoragePlugin _dataStoragePlugin;
        private readonly CollectionOfLogs _collectionOfLogs;
        private readonly DataStoragesHelperType _dataStoragesHelperType;

        public DevicesLogsRepository(DataStoragesHelperType dataStoragesHelper, CollectionOfLogs collectionOfLogs)
        {
            _dataStoragePlugin = dataStoragesHelper.GetDataStoragePlugin() ?? throw new ArgumentNullException(nameof(dataStoragesHelper));
            _collectionOfLogs = collectionOfLogs;

            _dataStoragesHelperType = dataStoragesHelper;
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
                //Debugger.Break();
                Console.WriteLine(ex.Message);

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
                //Debugger.Break();
                Console.WriteLine(ex.Message);
            }

            return true;
        }
    }
}
