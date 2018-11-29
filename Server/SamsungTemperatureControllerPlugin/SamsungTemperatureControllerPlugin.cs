using DataProviderCommon;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SamsungTemperatureControllerPlugin
{
    [Serializable]
    public class SamsungTemperatureControllerPlugin : IDevicePlugin
    {
        private const string GuidId = "CE9A8BDB-1E95-4517-B97C-754262401CB3";

        public string PluginName => "SamsungDPlugin";

        // Interface methods
        public DeviceLogs ConverterToStandard(string message)
        {
            JObject characteristicPart = JObject.Parse(message);
            var deviceData = characteristicPart["DeviceData"].ToObject<DeviceData>();

            return new DeviceLogs
            {
                DeviceGuid = GuidId,
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

        public bool PrepareDataForUI()
        {
            // TODO do all stuff to prepare data for ui
            return true;
        }
    }
}
