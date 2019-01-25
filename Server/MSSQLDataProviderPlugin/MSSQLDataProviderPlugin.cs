using DataProviderCommon;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MSSQLDataProviderPlugin;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MSSQlDataProviderPlugin
{
    public class MSSQLDataProviderPlugin : IDataStoragePlugin
    {
        private readonly MSSQLDbContext _dbContext;

        public string PluginName => "MSSQLDSPlugin";

        public string DisplayName => "MS SQL Server";


        public MSSQLDataProviderPlugin()
        {
            try {
                var builder = new ConfigurationBuilder()
               .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
               .AddJsonFile("msSqlDataProvider.json", optional: true, reloadOnChange: true);

                var configurator = builder.Build();

                var optionsBuilder = new DbContextOptionsBuilder<MSSQLDbContext>();

                optionsBuilder.UseSqlServer(configurator.GetConnectionString("msSQLConnectionString"));

                _dbContext = new MSSQLDbContext(optionsBuilder.Options);

                if (_dbContext.Database.GetPendingMigrations().Any())
                {
                    _dbContext.Database.Migrate();
                }
            }
            catch (Exception ex) {
                Debugger.Break();
            }
            

        }

        public IDataStorageOperationsOperations Operations => new Repository(_dbContext);

    }
}
