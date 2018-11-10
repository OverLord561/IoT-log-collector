using AppSettingsConfigurationPlugin.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace AppSettingsConfigurationPlugin
{
    public class AppSettingsConfiguration : IAppSettingsConfiguration
    {
        private IConfigurationRoot _configuration;

        private readonly string _jsonConfigFileName;

        public AppSettingsConfiguration(string jsonConfigFileName)
        {
            _jsonConfigFileName = jsonConfigFileName;

            ConfigureAppSettings();
        }

        private void ConfigureAppSettings()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile(_jsonConfigFileName, optional: true, reloadOnChange: true);

            _configuration = builder.Build();
        }

        public string GetValue(string key)
        {
            return _configuration.GetValue<string>(key);
        }

        public void GetSectionAndBind<T>(string key, T entity)
        {
            _configuration.Bind(key, entity);
        }
    }
}

