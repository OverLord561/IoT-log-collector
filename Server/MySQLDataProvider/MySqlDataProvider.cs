﻿using DataProviderFacade;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySQLDataProviderPlugin.Models;
using System;

using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace MySQLDataProviderPlugin
{
    public class MySqlDataProvider : IDataStoragePlugin
    {
        private readonly MySQLDbContext _dbContext;

        public MySqlDataProvider()
        {

            var builder = new ConfigurationBuilder()
               .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configurator = builder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<MySQLDbContext>();

            optionsBuilder.UseMySql(configurator.GetValue<string>("connectionString"));

            _dbContext = new MySQLDbContext(optionsBuilder.Options);
        }

        public IOperations Operations => new Repository(_dbContext);
    }
}
