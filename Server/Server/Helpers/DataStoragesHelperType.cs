using DataProviderCommon;
using Server.Models;
using Server.Services;
using Server.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Server.Helpers
{
    public class DataStoragesHelperType
    {
        private readonly IEnumerable<IDataStoragePlugin> _dataStoragePlugins;
        private ServerSettings _serverSettings;
        private readonly AppSettingsAccessor _appSettingsModifier;

        public DataStoragesHelperType(IEnumerable<IDataStoragePlugin> dataStoragePluginsCollection
            , AppSettingsAccessor appSettingsModifier
            )
        {
            _serverSettings = appSettingsModifier.GetServerSettings();
            _appSettingsModifier = appSettingsModifier;
            _dataStoragePlugins = dataStoragePluginsCollection;

            appSettingsModifier.NotifyDependentEntetiesEvent += HandleUserSettingsUpdate;
        }

        private void HandleUserSettingsUpdate()
        {
            _serverSettings = _appSettingsModifier.GetServerSettings();
        }

        public IDataStoragePlugin GetDataStoragePlugin()
        {
            var name = _serverSettings.DataStoragePlugin.Value;
            var ins = _dataStoragePlugins.FirstOrDefault(x => x.PluginName == name); 
            return ins;
        }

        public IEnumerable<DataStoragePluginViewModel> GetDataStoragePluginNames()
        {
            return _dataStoragePlugins.Select(x =>
                new DataStoragePluginViewModel
                {
                    DisplayName = x.DisplayName,
                    Value = x.PluginName,
                    IsSelected = x.PluginName.Equals(_serverSettings.DataStoragePlugin.Value)
                }
            );
        }
    }
}
