using DataProviderCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace XiomiPreasureControllerPlugin
{
    public class DeviceLogsUIFormat : IDeviceLogsUIFormat
    {
        public string DeviceName { get; set; }

        public List<ILog> Logs { get; set; }
    }
}
