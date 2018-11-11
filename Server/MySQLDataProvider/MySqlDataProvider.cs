using Microsoft.EntityFrameworkCore;
using System;

namespace MySQLDataProviderPlugin
{
    public class MySqlDataProvider
    {
        public readonly Action<DbContextOptionsBuilder> DbContextOptionsBuilder;

        public MySqlDataProvider(string connectionString)
        {
            DbContextOptionsBuilder = new Action<DbContextOptionsBuilder>(options =>
                options.UseMySql(connectionString));
        }
    }
}
