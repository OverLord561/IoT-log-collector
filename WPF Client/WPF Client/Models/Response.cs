using System.Collections.Generic;

namespace WPF_Client.Models
{
    public class Response
    {
        public int StatusCode { get; set; }
        public DeviceLogsInChartFormat ChartData { get; set; }
    }
}
