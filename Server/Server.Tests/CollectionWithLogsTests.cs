using DataProviderCommon;
using Microsoft.Extensions.Configuration;
using Server.Helpers;
using Server.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Server.Tests
{
    public class CollectionWithLogsTests
    {
        static IConfigurationRoot _config = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json")
                                .Build();

        DeviceLog _log = new DeviceLog { DateStamp = DateTime.Now, PluginName = "SamsungDPlugin" };

        CollectionOfLogs helperCollection = new CollectionOfLogs(_config);

        [Fact]
        public void SizeOfCollectionInParallelWriting()
        {
            // Arrange

            var countOfCollectios = 10;

            // Act

            var taskWriters = Enumerable.Range(1, 1000).Select(x =>
            {
                return Task.Run(() =>
                {
                    helperCollection.AddLog(_log);
                });
            }).ToArray();

            Task.WaitAll(taskWriters);

            //Assert
            Assert.Equal(countOfCollectios, helperCollection._allCollections.Count);
        }

        [Fact]
        public void SizeOfQueueInParallelWriting()
        {
            // Arrange

            var sizeOFQueue = 10;

            // Act

            var taskWriters = Enumerable.Range(1, 1000).Select(x =>
            {
                return Task.Run(() =>
                {
                    helperCollection.AddLog(_log);
                });
            }).ToArray();

            Task.WaitAll(taskWriters);

            //Assert
            Assert.Equal(sizeOFQueue, helperCollection._helperQueue.Count);
        }

        [Fact]
        public void CountOfLogsInEachCollection()
        {
            // Arrange
            var userSettings = new UserSettings();
            _config.Bind("userSettings", userSettings);

            var fullCollectionsCount = 10;
            var countOfLogsInEachCollection = userSettings.CapacityOfCollectionToInsert;
            // Act

            var taskWriters = Enumerable.Range(1, 1000).Select(x =>
            {
                return Task.Run(() =>
                {
                    helperCollection.AddLog(_log);
                });
            }).ToArray();

            Task.WaitAll(taskWriters);

            //Assert
            Assert.Equal(fullCollectionsCount, helperCollection._allCollections.Count(col => col.Count == countOfLogsInEachCollection));
        }

        [Fact]
        public void GetEmptyWorkingDirectory()
        {
            // Arrange
          
            // Act

            var taskWriters = Enumerable.Range(1, 1000).Select(x =>
            {
                return Task.Run(() =>
                {
                    helperCollection.AddLog(_log);
                });
            }).ToArray();

            Task.WaitAll(taskWriters);

            helperCollection._allCollections.FirstOrDefault().Clear();

            var workingCollection = helperCollection.GetWorkingCollection();

            //Assert
            Assert.Empty(workingCollection);
        }

        [Fact]
        public void GetCurrentWorkingDirectory()
        {
            // Arrange
            var userSettings = new UserSettings();
            _config.Bind("userSettings", userSettings);

            var countOfLogsInEachCollection = userSettings.CapacityOfCollectionToInsert;
            // Act

            var taskWriters = Enumerable.Range(1, 991).Select(x =>
            {
                return Task.Run(() =>
                {
                    helperCollection.AddLog(_log);
                });
            }).ToArray();

            Task.WaitAll(taskWriters);

            helperCollection._allCollections.FirstOrDefault(x=>x.Count < countOfLogsInEachCollection);

            var workingCollection = helperCollection.GetWorkingCollection();

            //Assert
            Assert.NotEmpty(workingCollection);
        }

        [Fact]
        public void GetExtraWorkingDirectory()
        {
            // Arrange

            // Act

            var taskWriters = Enumerable.Range(1, 1000).Select(x =>
            {
                return Task.Run(() =>
                {
                    helperCollection.AddLog(_log);
                });
            }).ToArray();

            Task.WaitAll(taskWriters);           

            var workingCollection = helperCollection.GetWorkingCollection();

            //Assert
            Assert.Empty(workingCollection);
        }

    }
}
