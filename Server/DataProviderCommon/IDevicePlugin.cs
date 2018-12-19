using System.Collections.Generic;

namespace DataProviderCommon
{
    public interface IDevicePlugin
    {
        string PluginName { get; }

        string PluginPurpose { get; }

        DeviceLog ConverterToStandard(string message);

        IDeviceLogsUIFormat PrepareDataForUI(List<DeviceLog> deviceLogs);
    }
}
