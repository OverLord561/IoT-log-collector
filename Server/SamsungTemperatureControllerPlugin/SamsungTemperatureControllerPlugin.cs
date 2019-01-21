using DataProviderCommon;
using System.Linq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SamsungTemperatureControllerPlugin
{
    [Serializable]
    public class SamsungTemperatureControllerPlugin : IDevicePlugin
    {
        public string PluginPurpose => "Temperature & Humidity";
        public string PluginName => "SamsungDPlugin";

        public string[] AxesNames => new string[3] { "Hour", "Temperature", "Humidity" };

        // Interface methods
        public DeviceLog ConverterToStandard(string message)
        {
            JObject characteristicPart = JObject.Parse(message);
            var deviceData = characteristicPart["DeviceData"].ToObject<DeviceData>();

            Random rendom = new Random();


            deviceData.Humidity = deviceData.Humidity * rendom.Next(1, 10);
            deviceData.Temperature = deviceData.Temperature * rendom.Next(1, 10);

            return new DeviceLog
            {
                PluginName = PluginName,
                DateStamp = DateTime.Now,
                Message = CharacteristicToByteArray(deviceData)
            };
        }

        public byte[] CharacteristicToByteArray(DeviceData deviceData)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();

                bf.Serialize(ms, deviceData);
                return ms.ToArray();
            }
        }

        public DeviceData ByteArrayToCharacteristics(byte[] message)
        {
            if (message == null)
                return default(DeviceData);

            using (MemoryStream ms = new MemoryStream(message))
            {
                BinaryFormatter bf = new BinaryFormatter();
                object obj = bf.Deserialize(ms);
                return (DeviceData)obj;
            }
        }

        public DeviceLogsInChartFormat PrepareDataForUI(List<DeviceLog> serializedLogs)
        {
            DeviceLogsInChartFormat uiData = new DeviceLogsInChartFormat
            {
                ChartName = this.PluginPurpose,
                Logs = new List<Log>(),
                AxesNames = AxesNames
            };

            DeserealizeLogsAndAddToChartsData(serializedLogs, uiData);

            return uiData;
        }

        private void DeserealizeLogsAndAddToChartsData(List<DeviceLog> serializedLogs, DeviceLogsInChartFormat uiData)
        {
            List<SamsungLog> samsungLogs = new List<SamsungLog>();

            foreach (var log in serializedLogs)
            {
                DeviceData deviceData = ByteArrayToCharacteristics(log.Message);

                samsungLogs.Add(new SamsungLog()
                {
                    Hour = log.DateStamp.Hour,
                    Temperature = deviceData.Temperature,
                    Humidity = deviceData.Humidity
                });
            }

            GroupLogsByHour(samsungLogs, uiData.Logs);
        }

        private void GroupLogsByHour(List<SamsungLog> samsungLogs, List<Log> logs)
        {
            samsungLogs = samsungLogs.GroupBy(log => log.Hour)
                      .OrderBy(log=> log.Key)
                      .Select(x => new SamsungLog
                      {
                          Hour = x.Key,
                          Temperature = Math.Round(x.Average(t => t.Temperature)),
                          Humidity = Math.Round(x.Average(t => t.Humidity))
                      })
                      .ToList();

            foreach (var sl in samsungLogs)
            {
                logs.Add(new Log
                {
                    Values = new double[3] { sl.Hour, sl.Temperature, sl.Humidity }
                });
            }
        }
    }
}
