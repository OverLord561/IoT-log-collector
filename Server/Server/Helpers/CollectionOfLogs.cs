using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataProviderCommon;
using Server.Models;

namespace Server.Helpers
{
    public class CollectionOfLogs
    {
        public ManualResetEventSlim resetEventSlim;
        static object _locker = new object();
        int _count = 5; // TODO: get it from appsettings.json

        List<MyTuple> _allTuples { get; set; }
        Queue<MyTuple> _helperQueue;

        public CollectionOfLogs()
        {
            resetEventSlim = new ManualResetEventSlim(false);

            MyTuple initialTuple = new MyTuple(new List<DeviceLog>(_count), null);

            _allTuples = new List<MyTuple>() { initialTuple };
            _helperQueue = new Queue<MyTuple>();
        }

        public void AddLog(DeviceLog log, IDataStoragePlugin plugin)
        {
            lock (_locker)
            {
                var workingTuple = GetWorkingTuple();

                workingTuple.Logs.Add(log);
                workingTuple.DataStoragePlugin = plugin;

            }
        }

        public MyTuple GetLogsToInsert()
        {
            lock (_locker)
            {
                if (_helperQueue.Any() && _helperQueue.Peek().Logs.Count == _count)
                {
                    var temporaryObj = new DeviceLog[_count];

                    var logsToInsert = _helperQueue.Peek();
                    logsToInsert.Logs.CopyTo(temporaryObj);
                    IDataStoragePlugin dataplugin = (IDataStoragePlugin)logsToInsert.DataStoragePlugin.Clone();

                    logsToInsert.Logs.Clear();

                    RemoveTupleFromQueue();

                    return new MyTuple(temporaryObj.ToList(), dataplugin);
                }
            }

            return null;
        }

        private MyTuple GetWorkingTuple()
        {

            lock (_locker)
            {
                var newCollection = new List<DeviceLog>(_count);

                var currentCollection = _allTuples.FirstOrDefault(tuple => tuple.Logs.Count != 0 && tuple.Logs.Count < _count); // current collection

                if (currentCollection != null)
                {
                    return currentCollection;
                }

                if (_allTuples.All(tuple => tuple.Logs.Count == _count)) // all collections all full
                {
                    MyTuple newTuple = new MyTuple(newCollection, null);
                    _allTuples.Add(newTuple);

                    AddTupleToQueue(newTuple);


                    return newTuple;
                }

                var emptyCollection = _allTuples.FirstOrDefault(tuple => !tuple.Logs.Any()); // return empty collection

                if (emptyCollection != null)
                {
                    AddTupleToQueue(emptyCollection);

                    return emptyCollection;
                }

                MyTuple newTuple2 = new MyTuple(newCollection, null);
                _allTuples.Add(newTuple2);

                AddTupleToQueue(newTuple2);

                return newTuple2;

            }

        }

        private void AddTupleToQueue(MyTuple myTuple)
        {
            lock (_locker)
            {
                _helperQueue.Enqueue(myTuple);
                if (!_helperQueue.Any())
                {
                    resetEventSlim.Reset();
                }
            }
        }


        private void RemoveTupleFromQueue()
        {
            lock (_locker)
            {
                _helperQueue.Dequeue();

                if (!_helperQueue.Any())
                {
                    resetEventSlim.Set();
                }
            }
        }
    }
}
