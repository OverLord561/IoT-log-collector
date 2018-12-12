using DataProviderCommon;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Repository
{
    public interface IDevicesLogsRepository
    {
        Task<List<DeviceLog>> GetDeviceLogsAsync(int? utcDate);

        Task<bool> WriteLog(string messageFromDevice, int? counter);
    }
}
