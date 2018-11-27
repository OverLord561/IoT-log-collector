namespace DataProviderCommon
{
    public interface IStandardizedDeviceOperations
    {
        byte[] ObjectToByteArray();

        StandardizedDevice ConverterToStandard();

        bool PrepareDataForUI();
    }
}
