using DataProviderFacade;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MSSQLDataProviderPlugin;
using System.IO;
using System.Reflection;

namespace MSSQlDataProviderPlugin
{
    public class MSSQLDataProvider : IDataStoragePlugin
    {
        private static readonly MSSQLDbContext _dbContext;

        static MSSQLDataProvider()
        {

            var builder = new ConfigurationBuilder()
               .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
               .AddJsonFile("msSqlDataProvider.json", optional: true, reloadOnChange: true);

            var configurator = builder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<MSSQLDbContext>();

            optionsBuilder.UseSqlServer(configurator.GetConnectionString("msSQLConnectionString"));

            _dbContext = new MSSQLDbContext(optionsBuilder.Options);

            _dbContext.Database.Migrate();

        }

        public IOperations Operations => new Repository(_dbContext);
    }
}
