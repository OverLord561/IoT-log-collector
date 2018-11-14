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

        public bool Add(GeneralDevice device)
        {
            _context.GeneralDevices.Add(device);

            return _context.SaveChanges() > 0;
        }

        public List<GeneralDevice> All()
        {
            return _context.GeneralDevices.ToList();

        }

        public List<GeneralDevice> Get(Expression<Func<GeneralDevice, bool>> predicate)
        {
            return _context.GeneralDevices.Where(predicate).ToList();
        }

        public bool Remove(GeneralDevice device)
        {
            throw new NotImplementedException();
        }
    }
}
