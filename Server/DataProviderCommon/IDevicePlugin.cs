using System.Collections.Generic;

namespace DataProviderCommon
{
    public interface IDevicePlugin
    {
        string PluginName { get; }

        DeviceLog ConverterToStandard(string message);

        IDeviceLogsUIFormat PrepareDataForUI(List<DeviceLog> deviceLogs);
    }
}
