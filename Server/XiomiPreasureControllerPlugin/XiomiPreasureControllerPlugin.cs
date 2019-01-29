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

        public string[] AxesNames => new string[2] { "Hour", "Preasure" };

        public string DisplayName => "Xiomi preasure";


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
            List<XiomiLog> XiomiLogs = new List<XiomiLog>();

            foreach (var log in serializedLogs)
            {
                DeviceData deviceData = ByteArrayToCharacteristics(log.Message);

                XiomiLogs.Add(new XiomiLog()
                {
                    Hour = log.DateStamp.Hour,
                    Preasure = deviceData.Preasure,
                });
            }

            GroupLogsByHour(XiomiLogs, uiData.Logs);
        }

        private void GroupLogsByHour(List<XiomiLog> XiomiLogs, List<Log> logs)
        {
            XiomiLogs = XiomiLogs.GroupBy(log => log.Hour)
                      .OrderBy(log => log.Key)
                      .Select(x => new XiomiLog
                      {
                          Hour = x.Key,
                          Preasure = Math.Round(x.Average(t => t.Preasure))
                      })
                      .ToList();

            foreach (var sl in XiomiLogs)
            {
                logs.Add(new Log
                {
                    Values = new double[2] { sl.Hour, sl.Preasure }
                });
            }
        }
    }
}
