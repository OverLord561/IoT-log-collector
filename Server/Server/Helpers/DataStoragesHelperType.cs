﻿using DataProviderCommon;
using Microsoft.Extensions.Options;
using Server.Models;
using Server.Services;
using Server.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Server.Helpers
{
    public class DataStoragesHelperType
    {
        private readonly IEnumerable<IDataStoragePlugin> _dataStoragePlugins;
        private UserSettings _userSettings;
        private readonly AppSettingsModifier _appSettingsModifier;

        public DataStoragesHelperType(IEnumerable<IDataStoragePlugin> dataStoragePluginsCollection
            , IOptions<UserSettings> subOptionsAccessor
            , AppSettingsModifier appSettingsModifier
            )
        {
            _userSettings = subOptionsAccessor.Value;
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
            return _dataStoragePlugins.FirstOrDefault(x => x.PluginName == _userSettings.DataProviderPluginName);
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
