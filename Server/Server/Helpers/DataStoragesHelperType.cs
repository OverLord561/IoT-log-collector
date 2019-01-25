using DataProviderCommon;
using Microsoft.Extensions.Options;
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
        private UserSettings _userSettings;
        private readonly AppSettingsAccessor _appSettingsModifier;

        public DataStoragesHelperType(IEnumerable<IDataStoragePlugin> dataStoragePluginsCollection            
            , AppSettingsAccessor appSettingsModifier
            )
        {
            _userSettings = appSettingsModifier.GetServerSettings();
            _appSettingsModifier = appSettingsModifier;
            _dataStoragePlugins = dataStoragePluginsCollection;

            appSettingsModifier.NotifyDependentEntetiesEvent += HandleUserSettingsUpdate;
        }

        private void HandleUserSettingsUpdate()
        {
            _userSettings = _appSettingsModifier.GetServerSettings();
        }

        public IDataStoragePlugin GetDataStoragePlugin()
        {
            var name = _userSettings.DataProviderPluginName;
            var ins = _dataStoragePlugins.FirstOrDefault(x => x.PluginName == name);
            if (ins == null) {
                Debugger.Break();
            }
           
            return ins;
        }

        public IEnumerable<DataStoragePluginViewModel> GetDataStoragePluginNames() {
            return _dataStoragePlugins.Select(x => 
                new DataStoragePluginViewModel {
                    DisplayName = x.DisplayName,
                    Value = x.PluginName,
                    IsSelected = x.PluginName.Equals(_userSettings.DataProviderPluginName)
                }
            );
        }
    }
}
