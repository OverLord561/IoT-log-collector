using DataProviderFacade;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySQLDataProviderPlugin.Models;
using System;
using System.IO;

namespace MySQLDataProviderPlugin
{
    public class MySqlDataProvider : IDataStoragePlugin
    {
        private static readonly IConfiguration _config;
        private readonly MySQLDbContext _dbContext;

        static MySqlDataProvider()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _config = builder.Build();
        }

        public MySqlDataProvider()
        {

            var optionsBuilder = new DbContextOptionsBuilder<MySQLDbContext>();
            optionsBuilder.UseMySql(_config.GetValue<string>("connectionString"));


            _dbContext = new MySQLDbContext(optionsBuilder.Options);
        }

        public IOperations Operations => new Repository(_dbContext);
    }
}
