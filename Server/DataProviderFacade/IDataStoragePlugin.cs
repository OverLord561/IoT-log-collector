using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DataProviderFacade
{
    public interface IDataStoragePlugin
    {
        IOperations Operations { get; }
    }
}
