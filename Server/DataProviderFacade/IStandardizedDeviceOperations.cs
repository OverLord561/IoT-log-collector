namespace DataProviderFacade
{
    public interface IStandardizedDeviceOperations
    {
        byte[] ObjectToByteArray();

        StandardizedDevice ConverterToStandard();

        bool PrepareDataForUI();
    }
}
