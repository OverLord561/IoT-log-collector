using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataProviderFacade
{
    public interface IOperations
    {
        bool Add(GeneralDevice device);

        bool Remove(GeneralDevice device);

        List<GeneralDevice> Get(Expression<Func<GeneralDevice, bool>> predicate);

        List<GeneralDevice> All();
    }
}
