using DataProviderCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace XiomiPreasureControllerPlugin
{
    public class Log : DeviceData, ILog
    {
        public int Hour { get; set; }
    }
}
