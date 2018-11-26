﻿using DataProviderFacade;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSSQlDataProviderPlugin
{
    public class MSSQLDbContext : DbContext
    {
        public MSSQLDbContext(DbContextOptions<MSSQLDbContext> options) : base(options) { }

        public DbSet<StandardizedDevice> GeneralDevices { get; set; }
    }
}