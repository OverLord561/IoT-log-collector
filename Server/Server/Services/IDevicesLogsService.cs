using DataProviderCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface IDevicesLogsService
    {
        List<IDeviceLogsUIFormat> PrepareLogsForUI(List<DeviceLog> logs);
    }
}
