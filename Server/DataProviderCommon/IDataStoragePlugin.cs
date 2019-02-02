namespace DataProviderCommon
{
    public interface IDataStoragePlugin
    {
        string PluginName { get;}

        string DisplayName { get;  }

        IDataStorageOperationsOperations Operations { get; }
    }
}
