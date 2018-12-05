using DataProviderCommon;
using Newtonsoft.Json;
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

        public IDeviceLogsUIFormat PrepareDataForUI(List<DeviceLog> logs)
        {
            //const data = [
            //  { name: 'Page A', uv: 4000, pv: 2400, amt: 2400 },
            //  { name: 'Page B', uv: 3000, pv: 1398, amt: 2210 },
            //  { name: 'Page C', uv: 2000, pv: 9800, amt: 2290 },
            //  { name: 'Page D', uv: 2780, pv: 3908, amt: 2000 },
            //  { name: 'Page E', uv: 1890, pv: 4800, amt: 2181 },
            //  { name: 'Page F', uv: 2390, pv: 3800, amt: 2500 },
            //  { name: 'Page G', uv: 3490, pv: 4300, amt: 2100 },
            //];

            DeviceLogsUIFormat data = new DeviceLogsUIFormat
            {
                DeviceName = this.PluginPurpose,
                Logs = new List<ILog>()
            };

            foreach (var log in logs)
            {
                DeviceData deviceData = ByteArrayToCharacteristics(log.Message);
                data.Logs.Add(new Log
                {
                    Hour = log.DateStamp.Hour,
                    Temperature = deviceData.Temperature,
                    Humidity = deviceData.Humidity
                });
            }

            return data;
        }
    }
}
