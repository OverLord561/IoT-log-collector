using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataProviderFacade
{
    public interface IOperations
    {
        bool Add(StandardizedDevice device);

        bool Remove(StandardizedDevice device);

        List<StandardizedDevice> Get(Expression<Func<StandardizedDevice, bool>> predicate);

        List<StandardizedDevice> All();
    }
}
