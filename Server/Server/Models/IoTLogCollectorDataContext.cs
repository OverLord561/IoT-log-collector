using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class IoTLogCollectorDataContext : DbContext
    {
        public IoTLogCollectorDataContext(DbContextOptions<IoTLogCollectorDataContext> options) : base(options) { }

        public DbSet<First> First { get; set; }

    }
}
