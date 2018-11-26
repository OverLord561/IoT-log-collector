using DataProviderFacade;
using MSSQlDataProviderPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MSSQLDataProviderPlugin
{
    class Repository : IOperations
    {
        private readonly MSSQLDbContext _context;

        public Repository(MSSQLDbContext context)
        {
            _context = context;
        }

        public bool Add(StandardizedDevice device)
        {
            _context.GeneralDevices.Add(device);

            return _context.SaveChanges() > 0;
        }

        public async Task<bool> AddAsync(StandardizedDevice device)
        {
            await _context.GeneralDevices.AddAsync(device);

            return await _context.SaveChangesAsync() > 0;
        }

        public bool AddRange(List<StandardizedDevice> standardizedDevices)
        {
            _context.GeneralDevices.AddRange(standardizedDevices);

            return _context.SaveChanges() > 0;
        }

        public async Task<bool> AddRangeAsync(List<StandardizedDevice> standardizedDevices)
        {
            await _context.GeneralDevices.AddRangeAsync(standardizedDevices);

            return await _context.SaveChangesAsync() > 0;
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
