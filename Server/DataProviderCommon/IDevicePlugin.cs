namespace DataProviderCommon
{
    public interface IDevicePlugin
    {
        StandardizedDevice ConverterToStandard(string message);

        bool PrepareDataForUI();
    }
}
