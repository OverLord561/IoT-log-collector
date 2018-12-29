using DataProviderCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface IDevicesLogsService
    {
        DeviceLogsInChartFormat PrepareLogsForUI(List<DeviceLog> logs, string deviceName);

        DeviceLog ConvertStringToDeviceLog(string message);
    }
}
