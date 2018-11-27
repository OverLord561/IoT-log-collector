using DataProviderCommon;
using Microsoft.Extensions.Configuration;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class DataStoragesHelperType
    {
        private readonly IConfiguration _configuration;
        private readonly IEnumerable<IDataStoragePlugin> _dataStoragePlugins;

        public DataStoragesHelperType(IConfiguration configuration, IEnumerable<IDataStoragePlugin> dataStoragePluginsCollection)
        {
            _configuration = configuration;
            _dataStoragePlugins = dataStoragePluginsCollection;
        }

        public IDataStoragePlugin GetDataStoragePlugin()
        {
            var userSettings = new UserSettings();
            _configuration.Bind("userSettings", userSettings);

            return _dataStoragePlugins.FirstOrDefault(x => x.GetType().Name == userSettings.DataProviderPluginName);
        }
    }
}
