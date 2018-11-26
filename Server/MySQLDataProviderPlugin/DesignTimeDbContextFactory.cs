using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace MySQLDataProviderPlugin
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MySQLDbContext>
    {
        public MySQLDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
               .AddJsonFile("mySqlDataProvider.json")
                .Build();

            var builder = new DbContextOptionsBuilder<MySQLDbContext>();
            var connectionString = configuration.GetConnectionString("mySQLConnectionString");
            builder.UseMySql(connectionString);
            return new MySQLDbContext(builder.Options);
        }
    }
}
