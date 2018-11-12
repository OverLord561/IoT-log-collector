using Microsoft.EntityFrameworkCore;
using System;

namespace DataProviderFacade
{
    public interface IDataProvider
    {
        Action<DbContextOptionsBuilder> DbContextOptionsBuilder { get; }
    }
}
