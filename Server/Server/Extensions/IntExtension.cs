﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Extensions
{
    public static class IntExtensions
    {
        public static DateTime FromUtcToLocalTime(this int seconds)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(seconds).ToLocalTime();
        }
       
    }
}
