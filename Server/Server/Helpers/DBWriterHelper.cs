using Microsoft.Extensions.Options;
using Server.Models;
using Server.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Helpers
{
    public class DBWriterHelper
    {
        private readonly CollectionOfLogs _collectionOfLogs;
        private readonly IDevicesLogsRepository _logsRepository;
        private readonly UserSettings _userSettings;

        public DBWriterHelper(CollectionOfLogs collectionOfLogs
            , IDevicesLogsRepository logsRepository
            , IOptions<UserSettings> subOptionsAccessor)
        {
            _collectionOfLogs = collectionOfLogs;
            _logsRepository = logsRepository;

            _userSettings = subOptionsAccessor.Value;
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
            });
        }

        private async Task<bool> WriteToDB()
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
