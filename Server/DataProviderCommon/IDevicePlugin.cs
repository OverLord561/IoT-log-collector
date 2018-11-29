namespace DataProviderCommon
{
    public interface IDevicePlugin
    {
        string PluginName { get; }

        DeviceLogs ConverterToStandard(string message);

        bool PrepareDataForUI();
    }
}
