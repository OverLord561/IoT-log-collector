using DataProviderCommon;
using Microsoft.Extensions.Options;
using Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Server.Helpers
{
    public class CollectionOfLogs
    {
        public readonly ManualResetEvent resetEvent;
        private readonly UserSettings _userSettings;
        readonly object _locker = new object();

        public List<List<DeviceLog>> _allCollections { get; }
        public Queue<List<DeviceLog>> _helperQueue { get; }

        public CollectionOfLogs(IOptions<UserSettings> subOptionsAccessor)
        {
            _userSettings = subOptionsAccessor.Value;
            resetEvent = new ManualResetEvent(false);

            List<DeviceLog> initialLogs = new List<DeviceLog>(_userSettings.CapacityOfCollectionToInsert);
            _allCollections = new List<List<DeviceLog>>() { initialLogs };
            _helperQueue = new Queue<List<DeviceLog>>();
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
                if (_helperQueue.Any() && _helperQueue.Peek().Count == _userSettings.CapacityOfCollectionToInsert)
                {
                    var temporaryObj = new DeviceLog[_userSettings.CapacityOfCollectionToInsert];

                    var collectionToInsert = _helperQueue.Peek();
                    collectionToInsert.CopyTo(temporaryObj);

                    collectionToInsert.Clear();

                    RemoveCollectionFromQueue();

                    return temporaryObj.ToList();
                }
            }

            return null;
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
