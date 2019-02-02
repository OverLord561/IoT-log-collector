using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataProviderCommon
{
    public interface IDataStorageOperationsOperations
    {
        bool Add(DeviceLog device);

        Task<bool> AddAsync(DeviceLog device);

        Task<bool> AddRangeAsync(List<DeviceLog> standardizedDevices);
        
        bool AddRange(List<DeviceLog> standardizedDevices);

        bool Remove(DeviceLog device);

        List<DeviceLog> Get(Expression<Func<DeviceLog, bool>> predicate);

        List<DeviceLog> All();

        Task<List<DeviceLog>> AllAsync();
    }
}
