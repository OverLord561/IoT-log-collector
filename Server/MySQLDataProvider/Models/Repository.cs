using DataProviderFacade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MySQLDataProviderPlugin.Models
{
    class Repository : IOperations
    {
        private readonly MySQLDbContext _context;

        public Repository(MySQLDbContext context)
        {
            _context = context;
        }

        public bool Add(StandardizedDevice device)
        {
            _context.GeneralDevices.Add(device);

            return _context.SaveChanges() > 0;
        }

        public List<StandardizedDevice> All()
        {
            return _context.GeneralDevices.ToList();

        }

        public List<StandardizedDevice> Get(Expression<Func<StandardizedDevice, bool>> predicate)
        {
            return _context.GeneralDevices.Where(predicate).ToList();
        }

        public bool Remove(StandardizedDevice device)
        {
            throw new NotImplementedException();
        }
    }
}
