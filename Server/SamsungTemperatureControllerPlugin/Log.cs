using DataProviderCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace SamsungTemperatureControllerPlugin
{
    public class Log : DeviceData, ILog
    {
        public int Hour { get; set; }
    }
}
