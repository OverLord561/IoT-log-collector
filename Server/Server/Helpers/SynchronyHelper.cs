using DataProviderCommon;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Helpers
{
    public class SynchronyHelper
    {
        public ManualResetEventSlim EventSlim { get; set; }

        public List<DeviceLog> Logs { get; }

        static object locker = new object();

        public SynchronyHelper()
        {
            EventSlim = new ManualResetEventSlim(false);
            Logs = new List<DeviceLog>(1000);
        }

        public bool AddLogToTemporaryList(DeviceLog log, IDataStoragePlugin dataStoragePlugin)
        {
            lock (locker)
            {
                Logs.Add(log);

                if (Logs.Count == 1000)
                {
                    var success = dataStoragePlugin.Operations.AddRange(Logs);
                    if (!success)
                    {
                        return false;
                    }

                    // if (succeeded)
                    //{
                    Logs.Clear();
                    EventSlim.Set();

                    return true;
                    
                }

                return true;
            }
        }
    }
}
