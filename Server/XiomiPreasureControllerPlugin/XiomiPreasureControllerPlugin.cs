using DataProviderCommon;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace XiomiPreasureControllerPlugin
{
    public class XiomiPreasureControllerPlugin : IDevicePlugin
    {
        public string PluginName => "XiomiDPlugin";

        public string PluginPurpose => "Preasure";

        // Interface methods
        public DeviceLog ConverterToStandard(string message)
        {
            JObject characteristicPart = JObject.Parse(message);
            var deviceData = characteristicPart["DeviceData"].ToObject<DeviceData>();

            Random rendom = new Random();
            deviceData.Preasure = deviceData.Preasure * rendom.Next(1, 3);

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
                    Preasure = deviceData.Preasure,
                });
            }

            var groupedByHour = deserealizedLogs.GroupBy(h => h.Hour);

            foreach (var logsPerHour in groupedByHour)
            {
                data.Logs.Add(new Log
                {
                    Hour = logsPerHour.Key,
                    Preasure = logsPerHour.Average(t => t.Preasure),
                });

            }

            return data;
        }
    }
}
