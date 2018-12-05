using DataProviderCommon;
using MSSQlDataProviderPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MSSQLDataProviderPlugin
{
    class Repository : IDataStorageOperationsOperations
    {
        private readonly MSSQLDbContext _context;

        public Repository(MSSQLDbContext context)
        {
            _context = context;
        }

        public bool Add(DeviceLog device)
        {
            _context.DeviceLogs.Add(device);

            return _context.SaveChanges() > 0;
        }

        public async Task<bool> AddAsync(DeviceLog device)
        {
            await _context.DeviceLogs.AddAsync(device);

            return await _context.SaveChangesAsync() > 0;
        }

        public bool AddRange(List<DeviceLog> standardizedDevices)
        {
            _context.DeviceLogs.AddRange(standardizedDevices);

            return _context.SaveChanges() > 0;
        }

        public async Task<bool> AddRangeAsync(List<DeviceLog> standardizedDevices)
        {
            await _context.DeviceLogs.AddRangeAsync(standardizedDevices);

            return await _context.SaveChangesAsync() > 0;
        }

        public List<DeviceLog> All()
        {
            return _context.DeviceLogs.ToList();

        }

        public List<DeviceLog> Get(Expression<Func<DeviceLog, bool>> predicate)
        {
            return _context.DeviceLogs.Where(predicate).ToList();
        }

        public bool Remove(DeviceLog device)
        {
            throw new NotImplementedException();
        }
    }
}
