using DataProviderCommon;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Server.Repository;

namespace Server.Helpers
{
    //this guy is Singleton
    public class DBWriterHelper
    {
        private readonly CollectionOfLogs _collectionOfLogs;
        private readonly IDevicesLogsRepository _logsRepository;

        public DBWriterHelper(CollectionOfLogs collectionOfLogs
            , IDevicesLogsRepository logsRepository
            )
        {
            _collectionOfLogs = collectionOfLogs;
            _logsRepository = logsRepository;

            WriteLogsToDbByInterval();
        }

        public void WriteLogsToDbByInterval()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(100); //TODO Get this from appsettings

                    WriteToDB().ConfigureAwait(false);
                }
            });
        }


        public async Task<bool> WriteToDB()
        {
            try
            {
                var collectionToInsert = _collectionOfLogs.GetLogsToInsert();

                if (collectionToInsert != null)
                {
                    return await _logsRepository.WriteRangeAsync(collectionToInsert).ConfigureAwait(false);
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
