using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataProviderCommon;
using Server.Extensions;

namespace Server.Repository
{
    public class DevicesLogsRepository : IDevicesLogsRepository
    {
        private readonly IDataStoragePlugin _dataStoragePlugin;

        public DevicesLogsRepository(DataStoragesHelperType dataStoragesHelper)
        {
            _dataStoragePlugin = dataStoragesHelper.GetDataStoragePlugin() ?? throw new ArgumentNullException(nameof(dataStoragesHelper));

        }
        public List<DeviceLog> GetDeviceLogs(int? utcDate)
        {
            var res = new List<DeviceLog>();

            if (utcDate == null)
            {
                res = _dataStoragePlugin.Operations.All();
            }
            else
            {
                DateTime _date = utcDate.Value.FromUtcToLocalTime();
                res = _dataStoragePlugin.Operations.Get(d => d.DateStamp.Day == _date.Day);
            }

            return res;
        }
    }
}
