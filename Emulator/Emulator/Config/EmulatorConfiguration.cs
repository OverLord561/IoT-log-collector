using AppSettingsConfigurationPlugin;
using Emulator.Config.Interfaces;
using Emulator.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Emulator.Config
{
    public class EmulatorConfiguration : IEmulatorConfiguration
    {
        private readonly MicrosoftConfiguration appSettingsConfiguration;

        public EmulatorConfiguration()
        {
            appSettingsConfiguration = new MicrosoftConfiguration("appsettings.json");
        }


        public ServerSettings GetServerSettings()
        {
            ServerSettings serverSettings = new ServerSettings();
            appSettingsConfiguration.config.Bind("serverSettings", serverSettings);

            return serverSettings;
        }
    }
}
