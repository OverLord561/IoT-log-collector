using DataProviderCommon;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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
        DeviceLog _log = new DeviceLog { DateStamp = DateTime.Now, PluginName = "SamsungDPlugin" };

        CollectionOfLogs helperCollection = new CollectionOfLogs(optionsAccessor);

        static IOptions<UserSettings> optionsAccessor = Options.Create(
           new UserSettings
           {
               DataProviderPluginName = "MySQLDSPlugin",
               CapacityOfCollectionToInsert = 100,
               IntervalForWritingIntoDb = 100
           });

        UserSettings _userSettings = optionsAccessor.Value;

        [Fact]
        public void CollectionOfCollections_After1000CallsFromApi_Contains10Collections()
        {
            // Arrange           
            var countOfCalls = 1000;
            var countOfCollectios = 10;

            // Act
            EmulateCalls(countOfCalls);

            //Assert
            Assert.Equal(countOfCollectios, helperCollection._allCollections.Count);
        }

        [Fact]
        public void CollectionHelper_After1000CallsFromApi_ContainsQueuWith10Elements()
        {
            // Arrange
            var countOfCalls = 1000;
            var countOFQueueElements = 10;

            // Act
            EmulateCalls(countOfCalls);

            //Assert
            Assert.Equal(countOFQueueElements, helperCollection._helperQueue.Count);
        }

        [Fact]
        public void CollectionOfCollections_After1000CallsFromApi_Contains10CollectionsWith100Logs()
        {
            // Arrange

            var countOfCalls = 1000;
            var fullCollectionsCount = 10;
            var countOfLogsInEachCollection = _userSettings.CapacityOfCollectionToInsert;

            // Act
            EmulateCalls(countOfCalls);

            //Assert
            Assert.Equal(fullCollectionsCount, helperCollection._allCollections.Count(col => col.Count == countOfLogsInEachCollection));
        }

        [Fact]
        public void CollectionsHelper_After1000CallsFromApiAndCleaningFirstCollection_ReturnsCurrentCleanedCollection()
        {
            // Arrange
            int countOfCalls = 1000;

            // Act
            EmulateCalls(countOfCalls);

            //helperCollection._allCollections.FirstOrDefault().Clear();

            var firstOrDefaultCollection = helperCollection._allCollections.FirstOrDefault();

            if (firstOrDefaultCollection == null) {
                throw new NotImplementedException();
            }

            firstOrDefaultCollection.Clear();

            var workingCollection = helperCollection.GetWorkingCollection();

            //Assert
            Assert.Empty(workingCollection);
        }

        [Fact]
        public void CollectionsHelper_After991CallsFromApi_ReturnsCurrentNotFullCollection()
        {
            // Arrange
            int countOfCalls = 991;
            var countOfLogsInEachCollection = _userSettings.CapacityOfCollectionToInsert;

            // Act
            EmulateCalls(countOfCalls);

            helperCollection._allCollections.FirstOrDefault(x => x.Count < countOfLogsInEachCollection);
            var workingCollection = helperCollection.GetWorkingCollection();

            //Assert
            Assert.NotEmpty(workingCollection);
        }

        [Fact]
        public void CollectionsHelper_After1000CallsFromApi_ReturnsOneMoreCollectionForOneMoreLog()
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
