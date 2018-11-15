using DataProviderFacade;
using Microsoft.EntityFrameworkCore;

namespace MySQLDataProviderPlugin
{
    public class MySQLDbContext : DbContext
    {
        public MySQLDbContext(DbContextOptions<MySQLDbContext> options) : base(options) { }

        public DbSet<GeneralDevice> GeneralDevices { get; set; }
    }

}
