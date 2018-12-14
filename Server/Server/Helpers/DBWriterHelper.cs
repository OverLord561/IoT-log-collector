using DataProviderCommon;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Server.Helpers
{
    //this guy is Singleton
    public class DBWriterHelper
    {       

        private readonly CollectionOfLogs _collectionOfLogs;

        public DBWriterHelper(CollectionOfLogs collectionOfLogs)
        {
            _collectionOfLogs = collectionOfLogs;            

            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);

                    WriteToDB().ConfigureAwait(false);
                }
            });

        }


        public bool AddToLogToCollection(DeviceLog deviceLog, IDataStoragePlugin dataStoragePlugin)
        {

            try
            {
                _collectionOfLogs.AddLog(deviceLog, dataStoragePlugin);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public async Task<bool> WriteToDB()
        {
            try
            {
                var tuple = _collectionOfLogs.GetLogsToInsert();

                if (tuple != null)
                {
                    //Console.WriteLine("Write to db \r\n");
                    return await tuple.DataStoragePlugin.Operations.AddRangeAsync(tuple.Logs).ConfigureAwait(false);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }
    }
}


