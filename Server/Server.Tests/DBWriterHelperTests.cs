using DataProviderCommon;
using Microsoft.Extensions.Options;
using Server.Helpers;
using Server.Models;
using Server.Services;
using Server.Tests.Mocks;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Server.Tests
{
    public class DBWriterHelperTests
    {
        private readonly CollectionOfLogs _helperCollection;
        private readonly DeviceLog _log;
        private readonly AppSettingsAccessor _appSettingsModifier;

        static object _locker = new object();

        public DBWriterHelperTests()
        {

            _log = new DeviceLog { DateStamp = DateTime.Now, PluginName = "SamsungDPlugin" };

            var options = Options
                .Create(new ServerSettings
                {
                    DataStoragePlugin = new ServerSettingViewModel
                    {
                        Name = "DataStoragePlugin",
                        Value = "MSSQLDSPlugin",
                        DisplayName = "Data storage plugin",
                        IsEditable = false
                    },
                    CapacityOfCollectionToInsert = new ServerSettingViewModel
                    {
                        Name = "BulkInsertCapacity",
                        DisplayName = "Bulk insert capacity",
                        Value = 100.ToString(),
                        IsEditable = true
                    },
                    IntervalForWritingIntoDb = new ServerSettingViewModel
                    {
                        Name = "BulkInsertInterval",
                        DisplayName = "Bulk insert interval",
                        Value = 100.ToString(),
                        IsEditable = true

                    }
                });

            _appSettingsModifier = new AppSettingsAccessor(options);

            _helperCollection = new CollectionOfLogs(_appSettingsModifier);
        }

        [Fact]
        public void InMemoryStorage_After1000CallsFromApi_Contains1000Elements()
        {
            // Arrange
            var repo = new DeviceLogsRepoMock();
            new LogsStorageWriter(_helperCollection, repo, _appSettingsModifier).RunLogsChecker(CancellationToken.None);
            var countOfCalls = 1000;

            // Act
            EmulateApiCalls(countOfCalls);

            var factCountInMemory = repo.logsInMemory.Count;

            //Assert
            Assert.Equal(countOfCalls, factCountInMemory);
        }

        [Fact]
        public void HelperQueue_AfterWrite_IsEmpty()
        {
            // Arrange
            var repo = new DeviceLogsRepoMock();
            new LogsStorageWriter(_helperCollection, repo, _appSettingsModifier).RunLogsChecker(CancellationToken.None);
            var countOfCalls = 1000;

            // Act
            EmulateApiCalls(countOfCalls);

            //Assert
            Assert.Empty(_helperCollection._helperQueue);
        }


        [Fact]
        public void HelperCollection_AfterWrite_ContainsEmptyLists()
        {
            // Arrange
            var repo = new DeviceLogsRepoMock();
            new LogsStorageWriter(_helperCollection, repo, _appSettingsModifier).RunLogsChecker(CancellationToken.None);
            var countOfCalls = 1000;

            // Act
            EmulateApiCalls(countOfCalls);
            bool res = _helperCollection._allCollections.All(x => !x.Any());
            //Assert
            Assert.True(res);
        }


        [Fact]
        public void InMemoryStorage_After1000CallsFromApi_ContainsElementsInTheSameOrderAsTheyCameFromApi()
        {
            // Arrange
            var repo = new DeviceLogsRepoMock();
            var countOfCalls = 1000;

            new LogsStorageWriter(_helperCollection, repo, _appSettingsModifier).RunLogsChecker(CancellationToken.None);
            var copyOfHelperCollectionAsList = new List<DeviceLog>();

            // Act

            var taskWriters = Enumerable.Range(1, countOfCalls).Select(x =>
            {
                return Task.Run(() =>
                {
                    var log = new DeviceLog { DateStamp = DateTime.Now, PluginName = "SamsungDPlugin", Id = x };

                    lock (_locker)
                    {
                        _helperCollection.AddLog(log);
                        copyOfHelperCollectionAsList.Add(log);
                    }

                });
            }).ToArray();

            Task.WaitAll(taskWriters); // full the collection of collections
            Thread.Sleep(3000);

            var isEqual = Enumerable.SequenceEqual(repo.logsInMemory, copyOfHelperCollectionAsList);

            //Assert
            Assert.True(isEqual);
        }

        private void EmulateApiCalls(int countOfLogs)
        {

            var taskWriters = Enumerable.Range(1, countOfLogs).Select(x =>
            {
                return Task.Run(() =>
                {
                    _helperCollection.AddLog(_log);
                });
            }).ToArray();

            Task.WaitAll(taskWriters);
            Thread.Sleep(2000); // wait for db writer helper
        }
    }
}
