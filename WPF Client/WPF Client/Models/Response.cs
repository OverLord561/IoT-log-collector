using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Client.Models
{
    public class Response
    {
        public int StatusCode { get; set; }
        public List<DeviceLogsUIFormat> Logs { get; set; }
    }
}
