using System;
using System.Collections.Generic;

namespace DataProviderCommon
{
    public interface IDevicePlugin
    {
        string PluginName { get; }

        string PluginPurpose { get; }

        DeviceLog ConverterToStandard(string message);

        DeviceLogsInChartFormat PrepareDataForUI(List<DeviceLog> deviceLogs);

        string[] AxesNames { get; }
    }
}
