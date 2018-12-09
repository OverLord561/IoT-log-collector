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
        private string PluginPurpose => "Temperature & Humidity";
        public string PluginName => "SamsungDPlugin";

        // Interface methods
        public DeviceLog ConverterToStandard(string message)
        {
            JObject characteristicPart = JObject.Parse(message);
            var deviceData = characteristicPart["DeviceData"].ToObject<DeviceData>();

            Random rendom = new Random();


            deviceData.Humidity = deviceData.Humidity * rendom.Next(1,10);
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

        public IDeviceLogsUIFormat PrepareDataForUI(List<DeviceLog> serializedLogs)
        {
            DeviceLogsUIFormat data = new DeviceLogsUIFormat
            {
                DeviceName = this.PluginPurpose,
                Logs = new List<ILog>()
            };

            var deserealizedLogs = new List<Log>();

            foreach (var log in serializedLogs)
            {
                DeviceData deviceData = ByteArrayToCharacteristics(log.Message);

                deserealizedLogs.Add(new Log
                {
                    Hour = log.DateStamp.Hour,
                    Temperature = deviceData.Temperature,
                    Humidity = deviceData.Humidity
                });
            }

            var groupedByHour = deserealizedLogs.GroupBy(h => h.Hour);

            foreach (var logsPerHour in groupedByHour) {
                var avrgTemperature = logsPerHour.Average(t => t.Temperature);

                data.Logs.Add(new Log
                {
                    Hour = logsPerHour.Key,
                    Temperature = logsPerHour.Average(t => t.Temperature),
                    Humidity = logsPerHour.Average(t => t.Humidity)
                });

            }

            return data;
        }
    }
}
