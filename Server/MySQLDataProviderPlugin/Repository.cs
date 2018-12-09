using DataProviderCommon;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<DeviceLog>> AllAsync()
        {
            return await _context.DeviceLogs.ToListAsync();
        }

        public List<DeviceLog> Get(Expression<Func<DeviceLog, bool>> predicate)
        {
            return _context.DeviceLogs.Where(predicate).ToList();
        }

        public bool Remove(DeviceLog device)
        {
            _context.DeviceLogs.Remove(device);

            return _context.SaveChanges() > 0;
        }
    }
}
