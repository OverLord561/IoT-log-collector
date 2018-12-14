using DataProviderCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class MyTuple
    {
        public MyTuple(List<DeviceLog> _logs, IDataStoragePlugin _dataStoragePlugin)
        {
            Logs = _logs;
            DataStoragePlugin = _dataStoragePlugin;
        }

        public List<DeviceLog> Logs { get; set; }

        public IDataStoragePlugin DataStoragePlugin { get; set; }

    }
}
