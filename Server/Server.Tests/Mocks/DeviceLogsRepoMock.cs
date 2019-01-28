using DataProviderCommon;
using Server.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Tests.Mocks
{
    public class DeviceLogsRepoMock : IDevicesLogsRepository
    {
        public List<DeviceLog> logsInMemory = new List<DeviceLog>();

        private readonly static object _locker = new object();

        public Task<List<DeviceLog>> GetDeviceLogsAsync(int? utcDate)
        {
            throw new NotImplementedException();
        }

        public bool WriteLogToTemporaryCollection(DeviceLog log)
        {
            throw new NotImplementedException();
        }

        public Task<bool> WriteRangeAsync(List<DeviceLog> logs)
        {
            lock (_locker)
            {
                Thread.Sleep(25);
                logsInMemory.AddRange(logs);
            }

            return Task.FromResult(true);
        }

        public bool WriteRange(List<DeviceLog> logs)
        {
            lock (_locker)
            {
                Thread.Sleep(25);
                logsInMemory.AddRange(logs);
            }

            return true;
        }
    }
}
