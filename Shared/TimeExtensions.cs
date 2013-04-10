using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renren.Components.Tools
{
    /// <summary>
    /// A collection of simple helper extension methods.
    /// </summary>
    public static class TimeExtensions
    {
        public static string ToUnixUtcTime(this DateTime now)
        {
            var utc = now.ToUniversalTime();
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0);
            var milliseconds = (long)utc.Subtract(start).TotalMilliseconds;

            return milliseconds.ToString();
        }
    }
}
