using Emulator.Config.Interfaces;
using Emulator.Models;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Emulator.Config
{
    public class EmulatorConfiguration : IEmulatorConfiguration
    {
        public readonly IConfiguration _config;

        public EmulatorConfiguration()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("emulatorAppsettings.json", optional: true, reloadOnChange: true);

            _config = builder.Build();
        }

        public ServerSettings GetServerSettings()
        {
            ServerSettings serverSettings = new ServerSettings();
            _config.Bind("serverSettings", serverSettings);

            return serverSettings;
        }
    }
}
