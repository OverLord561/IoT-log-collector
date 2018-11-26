using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace MSSQlDataProviderPlugin
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MSSQLDbContext>
    {
        public MSSQLDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
               .AddJsonFile("msSqlDataProvider.json")
                .Build();

            var builder = new DbContextOptionsBuilder<MSSQLDbContext>();
            var connectionString = configuration.GetConnectionString("msSQLConnectionString");
            builder.UseSqlServer(connectionString);
            return new MSSQLDbContext(builder.Options);
        }
    }
}
