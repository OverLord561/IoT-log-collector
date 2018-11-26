using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Extensions
{
    public static class Extensions
    {
        public static IEnumerable<T> Where<T>(this IEnumerable<T> data, Func<T, bool> predicate)
        {
            foreach (T value in data)
            {
                if (predicate(value)) yield return value;
            }
        }
    }
}
