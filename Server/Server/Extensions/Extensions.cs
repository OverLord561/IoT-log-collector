using DataProviderFacade;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Extensions
{
    public static class Extensions
    {
        public static T GetDataStorageProvider<T>(this IEnumerable<T> data) where T: IDataStoragePlugin
        {
            return data.FirstOrDefault();
        }
    }
}
