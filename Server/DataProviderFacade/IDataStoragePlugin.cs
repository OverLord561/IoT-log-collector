namespace DataProviderFacade
{
    public interface IDataStoragePlugin
    {
        IOperations Operations { get; }
    }
}
