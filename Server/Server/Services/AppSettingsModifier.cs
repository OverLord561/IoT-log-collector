using Newtonsoft.Json;
using Server.Models;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Server.Services
{
    public class AppSettingsModifier
    {
        readonly object _locker = new object();
        private UserSettings _userSettings;
        private string _filePath;

        public delegate void NotifyDependentEntetiesDel();
        public event NotifyDependentEntetiesDel NotifyDependentEntetiesEvent;

        public AppSettingsModifier()
        {
            _filePath = string.Concat(Directory.GetCurrentDirectory(), "\\appsettings.json");
        }

        public bool UpdateServerSettings(List<ServerSettingViewModel> serverSettings)
        {
            if (!ValidateServerSettings(serverSettings))
            {
                return false;
            }

            var appSettings = DeserealizeConfigFile();

            appSettings.UserSettings.CapacityOfCollectionToInsert = int.Parse(serverSettings.Find(x => x.Name.Equals("BulkInsertCapacity")).Value);
            appSettings.UserSettings.IntervalForWritingIntoDb = int.Parse(serverSettings.Find(x => x.Name.Equals("BulkInsertInterval")).Value);

            UpdateConfigFile(appSettings);

            return true;
        }

        public bool UpdateDataStoragePlugin(DataStoragePluginViewModel dataStoragePlugin)
        {
            var appSettings = DeserealizeConfigFile();

            appSettings.UserSettings.DataProviderPluginName = dataStoragePlugin.Value;
            UpdateConfigFile(appSettings);

            return true;
        }

        public UserSettings GetServerSettings()
        {
            lock (_locker)
            {
                return _userSettings;
            }
        }

        bool ValidateServerSettings(IEnumerable<ServerSettingViewModel> serverSettings)
        {

            return serverSettings.Skip(1)
                .All(x => int.TryParse(x.Value, out var res));
        }

        private AppSettings DeserealizeConfigFile()
        {
            return JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(_filePath));
        }
        private void UpdateConfigFile(AppSettings newAppSettings)
        {
            try
            {
                using (StreamWriter file = File.CreateText(_filePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, newAppSettings);
                }
            }
            catch (Exception ex)
            {
                Debugger.Break();
            }
            

            lock (_locker)
            {
                _userSettings = newAppSettings.UserSettings;
            }

            NotifyDependentEntetiesEvent();
        }
    }
}
