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
        public void Count_Of_Collection_Elements_In_Parallel_Writing_Should_Be_Digit()
        {
            // Arrange
            var userSettings = new UserSettings();
            _config.Bind("userSettings", userSettings);

            var countOfCalls = 1000;
            var countOfCollectios = countOfCalls / userSettings.CapacityOfCollectionToInsert;

            // Act
            EmulateCalls(countOfCalls);

            //Assert
            Assert.Equal(countOfCollectios, helperCollection._allCollections.Count);
        }

        [Fact]
        public void Count_Of_Queue_Elements_In_Parallel_Writing_Should_Be_Digit()
        {
            // Arrange
            var userSettings = new UserSettings();
            _config.Bind("userSettings", userSettings);

            var countOfCalls = 1000;
            var countOFQueueElements = countOfCalls / userSettings.CapacityOfCollectionToInsert;

            // Act
            EmulateCalls(countOfCalls);

            //Assert
            Assert.Equal(countOFQueueElements, helperCollection._helperQueue.Count);
        }

        [Fact]
        public void Count_Of_Logs_In_Each_Collection()
        {
            // Arrange
            var userSettings = new UserSettings();
            _config.Bind("userSettings", userSettings);

            var countOfCalls = 1000;
            var fullCollectionsCount = countOfCalls / userSettings.CapacityOfCollectionToInsert;
            var countOfLogsInEachCollection = userSettings.CapacityOfCollectionToInsert;

            // Act
            EmulateCalls(countOfCalls);

            //Assert
            Assert.Equal(fullCollectionsCount, helperCollection._allCollections.Count(col => col.Count == countOfLogsInEachCollection));
        }

        [Fact]
        public void Get_Empty_Working_Directory()
        {
            // Arrange
            int countOfCalls = 1000;

            // Act
            EmulateCalls(countOfCalls);

            helperCollection._allCollections.FirstOrDefault().Clear();
            var workingCollection = helperCollection.GetWorkingCollection();

            //Assert
            Assert.Empty(workingCollection);
        }

        [Fact]
        public void Get_Current_Working_Directory()
        {
            // Arrange
            var userSettings = new UserSettings();
            _config.Bind("userSettings", userSettings);
            int countOfCalls = 991;
            var countOfLogsInEachCollection = userSettings.CapacityOfCollectionToInsert;

            // Act
            EmulateCalls(countOfCalls);

            helperCollection._allCollections.FirstOrDefault(x => x.Count < countOfLogsInEachCollection);
            var workingCollection = helperCollection.GetWorkingCollection();

            //Assert
            Assert.NotEmpty(workingCollection);
        }

        [Fact]
        public void Get_Extra_Working_Directory()
        {
            // Arrange
            int countOfCalls = 1000;

            // Act
            EmulateCalls(countOfCalls);

            var workingCollection = helperCollection.GetWorkingCollection();

            //Assert
            Assert.Empty(workingCollection);
        }

        private void EmulateCalls(int countOfCalls)
        {

            var taskWriters = Enumerable.Range(1, countOfCalls).Select(x =>
            {
                return Task.Run(() =>
                {
                    helperCollection.AddLog(_log);
                });
            }).ToArray();

            Task.WaitAll(taskWriters);
        }
    }
}
