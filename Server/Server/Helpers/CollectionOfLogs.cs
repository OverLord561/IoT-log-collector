using DataProviderCommon;
using Microsoft.Extensions.Options;
using Server.Models;
using Server.Services;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Server.Helpers
{
    public class CollectionOfLogs
    {
        private readonly AppSettingsModifier _appSettingsModifier;

        public readonly ManualResetEvent resetEvent;
        private UserSettings _userSettings;
        readonly object _locker = new object();
        private bool AreUserSettingsUpdated;


        public List<List<DeviceLog>> _allCollections { get; }
        public Queue<List<DeviceLog>> _helperQueue { get; }

        public CollectionOfLogs(IOptions<UserSettings> subOptionsAccessor, AppSettingsModifier appSettingsModifier)
        {
            _userSettings = subOptionsAccessor.Value;
            resetEvent = new ManualResetEvent(false);

            _appSettingsModifier = appSettingsModifier;
            appSettingsModifier.NotifyDependentEntetiesEvent += HandleUserSettingsUpdate;

            List<DeviceLog> initialLogs = new List<DeviceLog>(_userSettings.CapacityOfCollectionToInsert);
            _allCollections = new List<List<DeviceLog>>() { initialLogs };
            _helperQueue = new Queue<List<DeviceLog>>();
        }

        private void HandleUserSettingsUpdate()
        {
            _userSettings = _appSettingsModifier.GetServerSettings();
            AreUserSettingsUpdated = true;
        }

        public bool AddLog(DeviceLog log)
        {
            lock (_locker)
            {
                var workingCollection = GetWorkingCollection();

                workingCollection.Add(log);
            }

            return true;
        }

        public List<DeviceLog> GetLogsToInsert()
        {
            lock (_locker)
            {
                if (AreUserSettingsUpdated || (_helperQueue.Any() && _helperQueue.Peek().Count == _userSettings.CapacityOfCollectionToInsert) )
                {
                    AreUserSettingsUpdated = false;
                    var temporaryObj = new DeviceLog[_userSettings.CapacityOfCollectionToInsert];

                    var collectionToInsert = _helperQueue.Peek();
                    collectionToInsert.CopyTo(temporaryObj);

                    collectionToInsert.Clear();

                    RemoveCollectionFromQueue();

                    return temporaryObj.ToList();
                }
            }

            return new List<DeviceLog>();
        }

        public List<DeviceLog> GetWorkingCollection()
        {
            lock (_locker)
            {
                var newEmptyCollection = new List<DeviceLog>(_userSettings.CapacityOfCollectionToInsert);

                var currentCollection = _allCollections
                                        .FirstOrDefault(collection => collection.Count != 0 && collection.Count < _userSettings.CapacityOfCollectionToInsert); // current collection

                if (currentCollection != null)
                {
                    return currentCollection;
                }

                var emptyCollection = _allCollections
                                        .FirstOrDefault(collection => !collection.Any()); // return empty collection

                if (emptyCollection != null)
                {
                    AddCollectionToQueue(emptyCollection);

                    return emptyCollection;
                }

                if (_allCollections.All(collection => collection.Count == _userSettings.CapacityOfCollectionToInsert)) // all collections all full add new 
                {
                    _allCollections.Add(newEmptyCollection);
                    AddCollectionToQueue(newEmptyCollection);

                    return newEmptyCollection;
                }

                _allCollections.Add(newEmptyCollection);
                AddCollectionToQueue(newEmptyCollection);

                return newEmptyCollection;
            }
        }

        private void AddCollectionToQueue(List<DeviceLog> collection)
        {
            lock (_locker)
            {
                _helperQueue.Enqueue(collection);
                if (!_helperQueue.Any())
                {
                    resetEvent.Reset();
                }
            }
        }

        private void RemoveCollectionFromQueue()
        {
            lock (_locker)
            {
                _helperQueue.Dequeue();

                if (!_helperQueue.Any())
                {
                    resetEvent.Set();
                }
            }
        }
    }
}
