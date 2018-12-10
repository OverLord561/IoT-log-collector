using DataProviderCommon;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace MySQLDataProviderPlugin
{
    public class MySQLDataProviderPlugin : IDataStoragePlugin
    {
        private readonly MySQLDbContext _dbContext;
        public string PluginName => "MySQLDSPlugin";

        public MySQLDataProviderPlugin()
        {

            var builder = new ConfigurationBuilder()
               .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
               .AddJsonFile("mySqlDataProvider.json", optional: true, reloadOnChange: true);

            var configurator = builder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<MySQLDbContext>();

            optionsBuilder.UseMySql(configurator.GetConnectionString("mySQLConnectionString"));

            _dbContext = new MySQLDbContext(optionsBuilder.Options);

            _dbContext.Database.Migrate();

        }

        public IDataStorageOperationsOperations Operations => new Repository(_dbContext);
    }
}
