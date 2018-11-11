using Microsoft.Extensions.Configuration;
using System.IO;

namespace AppSettingsConfigurationPlugin
{
    public class MicrosoftConfiguration
    {
        public readonly IConfiguration config;

        public MicrosoftConfiguration(string jsonConfigFileName)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile(jsonConfigFileName, optional: true, reloadOnChange: true);

            config = builder.Build();
        }
    }
}

