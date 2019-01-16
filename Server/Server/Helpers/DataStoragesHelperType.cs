using DataProviderCommon;
using Microsoft.Extensions.Options;
using Server.Models;
using System.Collections.Generic;
using System.Linq;

namespace Server.Helpers
{
    public class DataStoragesHelperType
    {
        private readonly IEnumerable<IDataStoragePlugin> _dataStoragePlugins;
        private readonly UserSettings _userSettings;

        public DataStoragesHelperType(IEnumerable<IDataStoragePlugin> dataStoragePluginsCollection, IOptions<UserSettings> subOptionsAccessor)
        {
            _userSettings = subOptionsAccessor.Value;
            _dataStoragePlugins = dataStoragePluginsCollection;
        }

        public IDataStoragePlugin GetDataStoragePlugin()
        {
            return _dataStoragePlugins.FirstOrDefault(x => x.PluginName == _userSettings.DataProviderPluginName);
        }
    }
}
