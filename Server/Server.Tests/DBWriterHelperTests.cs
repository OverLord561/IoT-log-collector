using DataProviderCommon;
using Microsoft.Extensions.Configuration;
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
        static IConfigurationRoot _config = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json")
                                .Build();

        DeviceLog _log = new DeviceLog { DateStamp = DateTime.Now, PluginName = "SamsungDPlugin" };
        CollectionOfLogs helperCollection = new CollectionOfLogs(_config);

        static object _locker = new object();


        [Fact]
        public void Add_1000_Logs_To_Collection_Of_Collections_And_Write_To_Storage()
        {
            // Arrange
            var repo = new DeviceLogsRepoMock();
            var dbWriter = new DBWriterHelper(helperCollection, repo, _config).RunLogsChecker(CancellationToken.None);
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
            var dbWriter = new DBWriterHelper(helperCollection, repo, _config).RunLogsChecker(CancellationToken.None);
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
            var dbWriter = new DBWriterHelper(helperCollection, repo, _config).RunLogsChecker(CancellationToken.None);
            var countOfCalls = 1000;

            // Act
            EmulateApiCalls(countOfCalls);
            var factCountInMemory = repo.logsInMemory.Count();
            var res = helperCollection._allCollections.All(x => !x.Any());
            //Assert
            Assert.True(res);
        }


        [Fact]
        public void Order_Of_Logs_In_Storage_Must_Be_The_Same_As_In_Adding_To_Helper_Collection()
        {
            // Arrange
            var repo = new DeviceLogsRepoMock();
            var countOfCalls = 1000;

            var userSettings = new UserSettings();
            _config.Bind("userSettings", userSettings);

            var dbWriter = new DBWriterHelper(helperCollection, repo, _config).RunLogsChecker(CancellationToken.None);
            var copyOfHelperCollectionAsList = new List<DeviceLog>();

            // Act

            var taskWriters = Enumerable.Range(1, countOfCalls).Select(x =>
            {
                return Task.Run(() =>
                {
                    DeviceLog _log = new DeviceLog { DateStamp = DateTime.Now, PluginName = "SamsungDPlugin", Id = x };
                    helperCollection.AddLog(_log);
                    lock (_locker)
                    {
                        copyOfHelperCollectionAsList.Add(_log);
                    }

                });
            }).ToArray();

            Task.WaitAll(taskWriters); // full the collection of collections
            Thread.Sleep(2000);

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
