﻿using DataProviderCommon;
using Microsoft.Extensions.Options;
using Server.Extensions;
using Server.Helpers;
using Server.Models;
using Server.Services;
using Server.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Server.Tests
{
    public class CollectionWithLogsTests
    {
        private readonly DeviceLog _log;
        private readonly CollectionOfLogs _helperCollection;
        private readonly AppSettingsAccessor _appSettingsModifier;

        public CollectionWithLogsTests()
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
        public void CollectionOfCollections_After1000CallsFromApi_Contains10Collections()
        {
            // Arrange           
            var countOfCalls = 1000;
            var countOfCollectios = 10;

            // Act
            EmulateCalls(countOfCalls);

            //Assert
            Assert.Equal(countOfCollectios, _helperCollection._allCollections.Count);
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
            Assert.Equal(countOFQueueElements, _helperCollection._helperQueue.Count);
        }

        [Fact]
        public void CollectionOfCollections_After1000CallsFromApi_Contains10CollectionsWith100Logs()
        {
            // Arrange

            var countOfCalls = 1000;
            var fullCollectionsCount = 10;
            var countOfLogsInEachCollection = _appSettingsModifier.GetServerSettings().CapacityOfCollectionToInsert.Value.ConvertToInt();

            // Act
            EmulateCalls(countOfCalls);

            //Assert
            Assert.Equal(fullCollectionsCount, _helperCollection._allCollections.Count(col => col.Count == countOfLogsInEachCollection));
        }

        [Fact]
        public void CollectionsHelper_After1000CallsFromApiAndCleaningFirstCollection_ReturnsCurrentCleanedCollection()
        {
            // Arrange
            int countOfCalls = 1000;

            // Act
            EmulateCalls(countOfCalls);

            var firstOrDefaultCollection = _helperCollection._allCollections.FirstOrDefault();

            if (firstOrDefaultCollection == null)
            {
                throw new NotImplementedException();
            }

            firstOrDefaultCollection.Clear();

            var workingCollection = _helperCollection.GetWorkingCollection();

            //Assert
            Assert.Empty(workingCollection);
        }

        [Fact]
        public void CollectionsHelper_After991CallsFromApi_ReturnsCurrentNotFullCollection()
        {
            // Arrange
            int countOfCalls = 991;
            var countOfLogsInEachCollection = _appSettingsModifier.GetServerSettings().CapacityOfCollectionToInsert.Value.ConvertToInt();

            // Act
            EmulateCalls(countOfCalls);

            _helperCollection._allCollections.FirstOrDefault(x => x.Count < countOfLogsInEachCollection);
            var workingCollection = _helperCollection.GetWorkingCollection();

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

            var workingCollection = _helperCollection.GetWorkingCollection();

            //Assert
            Assert.Empty(workingCollection);
        }

        private void EmulateCalls(int countOfCalls)
        {

            var taskWriters = Enumerable.Range(1, countOfCalls).Select(x =>
            {
                return Task.Run(() =>
                {
                    _helperCollection.AddLog(_log);
                });
            }).ToArray();

            Task.WaitAll(taskWriters);
        }
    }
}
