using DataProviderCommon;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Server.Helpers;
using Server.Models;
using Server.Tests.Mocks;
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

        CollectionOfLogs helperCollection = new CollectionOfLogs(optionsAccessor);
        static object _locker = new object();

        DeviceLog _log = new DeviceLog { DateStamp = DateTime.Now, PluginName = "SamsungDPlugin" };
        static IOptions<UserSettings> optionsAccessor = Options.Create(
            new UserSettings
            {
                DataProviderPluginName = "MySQLDSPlugin",
                CapacityOfCollectionToInsert = 100,
                IntervalForWritingIntoDb = 100
            });

        [Fact]
        public void InMemoryStorage_After1000CallsFromApi_Contains1000Elements()
        {
            // Arrange
            var repo = new DeviceLogsRepoMock();
            var dbWriter = new DBWriterHelper(helperCollection, repo, optionsAccessor).RunLogsChecker(CancellationToken.None);
            var countOfCalls = 1000;

            // Act
            EmulateApiCalls(countOfCalls);

            var factCountInMemory = repo.logsInMemory.Count();

            //Assert
            Assert.Equal(countOfCalls, factCountInMemory);
        }

        [Fact]
        public void HelperQueue_AfterWrite_IsEmpty()
        {
            // Arrange
            var repo = new DeviceLogsRepoMock();
            var dbWriter = new DBWriterHelper(helperCollection, repo, optionsAccessor).RunLogsChecker(CancellationToken.None);
            var countOfCalls = 1000;

            // Act
            EmulateApiCalls(countOfCalls);
            var factCountInMemory = repo.logsInMemory.Count();

            //Assert
            Assert.Empty(helperCollection._helperQueue);
        }


        [Fact]
        public void HelperCollection_AfterWrite_ContainsEmptyLists()
        {
            // Arrange
            var repo = new DeviceLogsRepoMock();
            var dbWriter = new DBWriterHelper(helperCollection, repo, optionsAccessor).RunLogsChecker(CancellationToken.None);
            var countOfCalls = 1000;

            // Act
            EmulateApiCalls(countOfCalls);
            var factCountInMemory = repo.logsInMemory.Count();
            var res = helperCollection._allCollections.All(x => !x.Any());
            //Assert
            Assert.True(res);
        }


        [Fact]
        public void InMemoryStorage_After1000CallsFromApi_ContainsElementsInTheSameOrderAsTheyCameFromApi()
        {
            // Arrange
            var repo = new DeviceLogsRepoMock();
            var countOfCalls = 1000;

            var dbWriter = new DBWriterHelper(helperCollection, repo, optionsAccessor).RunLogsChecker(CancellationToken.None);
            var copyOfHelperCollectionAsList = new List<DeviceLog>();

            // Act

            var taskWriters = Enumerable.Range(1, countOfCalls).Select(x =>
            {
                return Task.Run(() =>
                {
                    DeviceLog _log = new DeviceLog { DateStamp = DateTime.Now, PluginName = "SamsungDPlugin", Id = x };
                  
                    lock (_locker)
                    {
                        helperCollection.AddLog(_log);
                        copyOfHelperCollectionAsList.Add(_log);
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
                    helperCollection.AddLog(_log);
                });
            }).ToArray();

            Task.WaitAll(taskWriters);
            Thread.Sleep(2000); // wait for db writer helper
        }
    }
}
