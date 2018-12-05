using System;
using System.Collections.Generic;
using System.Text;

namespace DataProviderCommon
{
    public interface IDeviceLogsUIFormat
    {
        string DeviceName { get; set; }

        List<ILog> Logs { get; set; }
    }

    public interface ILog
    {
    }
}
