using System;
using System.Collections.Generic;

namespace DataProviderCommon
{
    public class DeviceLogsInChartFormat
    {
        public string ChartName { get; set; }

        public string[] AxesNames { get; set; }

        public List<Log> Logs { get; set; }
    }

    public class Log
    {
        public double[] Values { get; set; }
    }
}
