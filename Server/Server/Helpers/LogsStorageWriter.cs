using Server.Extensions;
using Server.Models;
using Server.Repository;
using Server.Services;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Helpers
{
    public class LogsStorageWriter
    {
        private readonly AppSettingsAccessor _appSettingsModifier;
        private readonly CollectionOfLogs _collectionOfLogs;
        private readonly IDevicesLogsRepository _logsRepository;
        private ServerSettings _serverSettings;
        private readonly AutoResetEvent _autoResetEvent;

        public LogsStorageWriter(CollectionOfLogs collectionOfLogs
            , IDevicesLogsRepository logsRepository
            , AppSettingsAccessor appSettingsModifier
            )
        {
            _collectionOfLogs = collectionOfLogs;
            _logsRepository = logsRepository;

            _appSettingsModifier = appSettingsModifier;
            appSettingsModifier.NotifyDependentEntetiesEvent += HandleUserSettingsUpdate;

            _serverSettings = appSettingsModifier.GetServerSettings();

            _autoResetEvent = new AutoResetEvent(false);
        }

        public Task RunLogsChecker(CancellationToken appCancellationToken)
        {
            return Task.Run(async () =>
            {
                while (!appCancellationToken.IsCancellationRequested)
                {

                    //await Task.Delay(TimeSpan.FromMilliseconds(_serverSettings.IntervalForWritingIntoDb.Value.ConvertToInt()));
                    _autoResetEvent.WaitOne(_serverSettings.IntervalForWritingIntoDb.Value.ConvertToInt());

                    await WriteToDBAsync();
                }
            }, appCancellationToken);
        }

        private async Task<bool> WriteToDBAsync()
        {
            try
            {
                var collectionToInsert = _collectionOfLogs.GetLogsToInsert();

                if (collectionToInsert.Any())
                {

                    //var res = await _logsRepository.WriteRangeAsync(collectionToInsert);
                    //return res;

                    return await _logsRepository.WriteRangeAsync(collectionToInsert);

                }

            }
            catch (Exception ex)
            {
                //Debugger.Break();
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        private void HandleUserSettingsUpdate()
        {
            _serverSettings = _appSettingsModifier.GetServerSettings();
            _autoResetEvent.Set();
        }
    }
}
