using DataProviderCommon;
using Microsoft.Extensions.Options;
using Server.Extensions;
using Server.Models;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Server.Helpers
{
    public class CollectionOfLogs
    {
        public readonly ManualResetEvent resetEvent;

        private readonly AppSettingsAccessor _appSettingsModifier;
        private ServerSettings _serverSettings;
        readonly object _locker = new object();
        private bool AreUserSettingsUpdated;


        public List<List<DeviceLog>> _allCollections { get; }
        public Queue<List<DeviceLog>> _helperQueue { get; }

        public CollectionOfLogs(AppSettingsAccessor appSettingsModifier)
        {
            _serverSettings = appSettingsModifier.GetServerSettings();
            resetEvent = new ManualResetEvent(false);

            _appSettingsModifier = appSettingsModifier;
            appSettingsModifier.NotifyDependentEntetiesEvent += HandleUserSettingsUpdate;

            List<DeviceLog> initialLogs = new List<DeviceLog>(_serverSettings.CapacityOfCollectionToInsert.Value.ConvertToInt());
            _allCollections = new List<List<DeviceLog>>() { initialLogs };
            _helperQueue = new Queue<List<DeviceLog>>();
        }

        private void HandleUserSettingsUpdate()
        {
            _serverSettings = _appSettingsModifier.GetServerSettings();
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
            try
            {
                lock (_locker)
                {
                    if (_helperQueue.Any() && ((AreUserSettingsUpdated) || (_helperQueue.Peek().Count == _serverSettings.CapacityOfCollectionToInsert.Value.ConvertToInt())))
                    {
                        AreUserSettingsUpdated = false;

                        var collectionToInsert = _helperQueue.Peek();
                        var temporaryObj = new DeviceLog[collectionToInsert.Count];

                        collectionToInsert.CopyTo(temporaryObj);

                        collectionToInsert.Clear();

                        RemoveCollectionFromQueue();

                        return temporaryObj.Where(x => x != null).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                //Debugger.Break();
                Console.WriteLine(ex.Message);
            }

            return new List<DeviceLog>();
        }

        public List<DeviceLog> GetWorkingCollection()
        {
            lock (_locker)
            {
                try
                {
                    var newEmptyCollection = new List<DeviceLog>(_serverSettings.CapacityOfCollectionToInsert.Value.ConvertToInt());

                    var currentCollection = _allCollections
                                            .FirstOrDefault(collection => collection.Count != 0 && collection.Count < _serverSettings.CapacityOfCollectionToInsert.Value.ConvertToInt()); // current collection

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

                    if (_allCollections.All(collection => collection.Count == _serverSettings.CapacityOfCollectionToInsert.Value.ConvertToInt())) // all collections all full add new 
                    {
                        _allCollections.Add(newEmptyCollection);
                        AddCollectionToQueue(newEmptyCollection);

                        return newEmptyCollection;
                    }

                    _allCollections.Add(newEmptyCollection);
                    AddCollectionToQueue(newEmptyCollection);

                    return newEmptyCollection;
                }
                catch (Exception ex)
                {
                    //Debugger.Break();
                    Console.WriteLine(ex.Message);

                    return new List<DeviceLog>(10000);
                }
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
