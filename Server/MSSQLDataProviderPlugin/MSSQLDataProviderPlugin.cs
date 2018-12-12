using DataProviderCommon;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MSSQLDataProviderPlugin;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MSSQlDataProviderPlugin
{
    public class MSSQLDataProviderPlugin : IDataStoragePlugin
    {
        private readonly MSSQLDbContext _dbContext;

        public string PluginName => "MSSQLDSPlugin";


        public MSSQLDataProviderPlugin()
        {

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

        public IDataStorageOperationsOperations Operations => new Repository(_dbContext);

    }
}
