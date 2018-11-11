using MySQLDataProviderPlugin.Repository;
using Server.Models;
using Server.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Repositories
{
    public class FirstRepository<TEntity> : Repository<First>, IFirstRepository
    {

        public FirstRepository(IoTLogCollectorDataContext context) : base(context)
        {

        }

        protected override IQueryable<First> Include()
        {
            return base.Include();
        }
    }
}
