using Microsoft.Extensions.Options;
using Server.Models;
using Server.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Server.Services;
using System.Diagnostics;

namespace Server.Helpers
{
    public class LogsStorageWriter
    {
        private readonly AppSettingsAccessor _appSettingsModifier;
        private readonly CollectionOfLogs _collectionOfLogs;
        private readonly IDevicesLogsRepository _logsRepository;
        private UserSettings _userSettings;


        public LogsStorageWriter(CollectionOfLogs collectionOfLogs
            , IDevicesLogsRepository logsRepository            
            , AppSettingsAccessor appSettingsModifier
            )
        {
            _collectionOfLogs = collectionOfLogs;
            _logsRepository = logsRepository;

            _appSettingsModifier = appSettingsModifier;
            appSettingsModifier.NotifyDependentEntetiesEvent += HandleUserSettingsUpdate;

            _userSettings = appSettingsModifier.GetServerSettings();
        }

        public Task RunLogsChecker(CancellationToken appCancellationToken)
        {
            return Task.Run(async () =>
            {
                while (!appCancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(_userSettings.IntervalForWritingIntoDb));

                    await WriteToDB();
                }
            }, appCancellationToken);
        }

        private async Task<bool> WriteToDB()
        {
            try
            {
                var collectionToInsert = _collectionOfLogs.GetLogsToInsert();

                if (collectionToInsert.Any())
                {
                    _appSettingsModifier.NotifyDependentEntetiesEvent -= HandleUserSettingsUpdate;
                    return await _logsRepository.WriteRangeAsync(collectionToInsert).ConfigureAwait(false);
                }

            }
            catch (Exception ex)
            {
                Debugger.Break();
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        private async void HandleUserSettingsUpdate()
        {
            _userSettings = _appSettingsModifier.GetServerSettings();

            await WriteToDB();
        }
    }
}
