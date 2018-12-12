using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataProviderCommon;
using Newtonsoft.Json;
using Server.Extensions;
using Server.Helpers;

namespace Server.Repository
{
    public class DevicesLogsRepository : IDevicesLogsRepository
    {
        private readonly IDataStoragePlugin _dataStoragePlugin;
        private readonly SynchronyHelper _synchronyHelper;
        private readonly DeviceHelperType _devicePluginsHelper;


        public DevicesLogsRepository(DataStoragesHelperType dataStoragesHelper, SynchronyHelper synchronyHelper, DeviceHelperType deviceHelperType)
        {
            _dataStoragePlugin = dataStoragesHelper.GetDataStoragePlugin() ?? throw new ArgumentNullException(nameof(dataStoragesHelper));
            _synchronyHelper = synchronyHelper;
            _devicePluginsHelper = deviceHelperType;

        }
        public async Task<List<DeviceLog>> GetDeviceLogsAsync(int? utcDate)
        {
            var res = new List<DeviceLog>();

            if (utcDate == null)
            {
                res = await _dataStoragePlugin.Operations.AllAsync();
            }
            else
            {
                DateTime _date = utcDate.Value.FromUtcToLocalTime();
                res = _dataStoragePlugin.Operations.Get(d => d.DateStamp.Day == _date.Day);
            }

            return res;
        }

        public async Task<bool> WriteLog(string messageFromDevice, int? counter = 0)
        {
            var generalPluginInfo = JsonConvert.DeserializeObject<StandardizedMessageFromDevice>(messageFromDevice);

            var plugin = _devicePluginsHelper.GetDevicePlugin(generalPluginInfo.PluginName); /* SamsungDPlugin*/

            if (plugin == null)
            {
                throw new ArgumentNullException(nameof(plugin));
            }

            var standardizedDevice = plugin.ConverterToStandard(messageFromDevice);
            standardizedDevice.DateStamp = standardizedDevice.DateStamp.AddHours(counter.Value);

            var res = _synchronyHelper.AddLogToTemporaryList(standardizedDevice, _dataStoragePlugin);            

            return res;
        }
    }
}
