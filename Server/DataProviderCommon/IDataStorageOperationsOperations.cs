using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataProviderCommon
{
    public interface IDataStorageOperationsOperations
    {
        bool Add(DeviceLogs device);

        Task<bool> AddAsync(DeviceLogs device);

        Task<bool> AddRangeAsync(List<DeviceLogs> standardizedDevices);
        
        bool AddRange(List<DeviceLogs> standardizedDevices);


        bool Remove(DeviceLogs device);

        List<DeviceLogs> Get(Expression<Func<DeviceLogs, bool>> predicate);

        List<DeviceLogs> All();
    }
}
