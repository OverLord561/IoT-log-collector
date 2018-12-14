using System;

namespace DataProviderCommon
{
    public interface IDataStoragePlugin: ICloneable
    {
        string PluginName { get;}

        IDataStorageOperationsOperations Operations { get; }
    }
}
