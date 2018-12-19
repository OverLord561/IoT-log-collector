using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Client.Models
{
    public class DeviceLogsUIFormat
    {
        public string DeviceName { get; set; }

        public List<Log> Logs { get; set; }
    }
}
