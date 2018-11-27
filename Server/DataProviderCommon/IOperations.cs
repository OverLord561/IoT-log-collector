using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataProviderCommon
{
    public interface IOperations
    {
        bool Add(StandardizedDevice device);

        Task<bool> AddAsync(StandardizedDevice device);

        Task<bool> AddRangeAsync(List<StandardizedDevice> standardizedDevices);
        
        bool AddRange(List<StandardizedDevice> standardizedDevices);


        bool Remove(StandardizedDevice device);

        List<StandardizedDevice> Get(Expression<Func<StandardizedDevice, bool>> predicate);

        List<StandardizedDevice> All();
    }
}
