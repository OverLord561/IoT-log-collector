using DataProviderCommon;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Server.Repository;
using Microsoft.Extensions.Configuration;
using Server.Models;

namespace Server.Helpers
{
    //this guy is Singleton
    public class DBWriterHelper
    {
        private readonly CollectionOfLogs _collectionOfLogs;
        private readonly IDevicesLogsRepository _logsRepository;
        private readonly IConfiguration _config;

        public DBWriterHelper(CollectionOfLogs collectionOfLogs
            , IDevicesLogsRepository logsRepository,
            IConfiguration configuration
            )
        {
            _collectionOfLogs = collectionOfLogs;
            _logsRepository = logsRepository;
            _config = configuration;

            WriteLogsToDbByInterval();
        }

        public void WriteLogsToDbByInterval()
        {
            Task.Run(() =>
            {
                var userSettings = new UserSettings();
                _config.Bind("userSettings", userSettings);

                while (true)
                {
                    Thread.Sleep(userSettings.IntervalForWritingIntoDb); //TODO Get this from appsettings

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
