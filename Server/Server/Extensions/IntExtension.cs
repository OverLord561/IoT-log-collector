using System;

namespace Server.Extensions
{
    public static class IntExtensions
    {
        public static DateTime FromUnixToLocalTime(this int seconds)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(seconds).ToLocalTime();
        }
       
    }
}
