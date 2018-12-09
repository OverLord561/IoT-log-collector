using DataProviderCommon;
using System.Collections.Generic;

namespace Server.Repository
{
    public interface IDevicesLogsRepository
    {
        List<DeviceLog> GetDeviceLogs(int? utcDate);
    }
}
