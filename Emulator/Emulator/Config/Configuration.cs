using AppSettingsConfigurationPlugin;
using Emulator.Config.Interfaces;
using Emulator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Emulator.Config
{
    public class Configuration : IConfiguration
    {
        private readonly AppSettingsConfiguration appSettingsConfiguration;

        public Configuration()
        {
            appSettingsConfiguration = new AppSettingsConfiguration("appsettings.json");
        }


        public ServerSettings GetServerSettings()
        {
            ServerSettings serverSettings = new ServerSettings();
            appSettingsConfiguration.GetSectionAndBind<ServerSettings>("serverSettings", serverSettings);

            return serverSettings;
        }
    }
}
