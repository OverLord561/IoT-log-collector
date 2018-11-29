using DataProviderCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MySQLDataProviderPlugin
{
    public class Repository : IDataStorageOperationsOperations
    {
        private readonly MySQLDbContext _context;

        public Repository(MySQLDbContext context)
        {
            _context = context;
        }

        public bool Add(DeviceLogs device)
        {
            _context.DeviceLogs.Add(device);

            return _context.SaveChanges() > 0;
        }

        public async Task<bool> AddAsync(DeviceLogs device)
        {
            await _context.DeviceLogs.AddAsync(device);

            return await _context.SaveChangesAsync() > 0;
        }

        public bool AddRange(List<DeviceLogs> standardizedDevices)
        {
            _context.DeviceLogs.AddRange(standardizedDevices);

            return _context.SaveChanges() > 0;
        }

        public async Task<bool> AddRangeAsync(List<DeviceLogs> standardizedDevices)
        {
            await _context.DeviceLogs.AddRangeAsync(standardizedDevices);

            return await _context.SaveChangesAsync() > 0;
        }

        public List<DeviceLogs> All()
        {
            return _context.DeviceLogs.ToList();

        }

        public List<DeviceLogs> Get(Expression<Func<DeviceLogs, bool>> predicate)
        {
            return _context.DeviceLogs.Where(predicate).ToList();
        }

        public bool Remove(DeviceLogs device)
        {
            throw new NotImplementedException();
        }
    }
}
