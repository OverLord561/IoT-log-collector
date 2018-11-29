namespace DataProviderCommon
{
    public interface IDataStoragePlugin
    {
        string PluginName { get;}

        IDataStorageOperationsOperations Operations { get; }
    }
}
