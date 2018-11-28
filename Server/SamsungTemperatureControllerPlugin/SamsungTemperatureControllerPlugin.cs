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
        private const string Name = "Samsung_RT38F";
        private DeviceCharacteristics DeviceCharacteristics;

        // Interface methods
        public StandardizedDevice ConverterToStandard(string message)
        {
            JObject characteristicPart = JObject.Parse(message);
            DeviceCharacteristics = characteristicPart["DeviceCharacteristics"].ToObject<DeviceCharacteristics>();

            return new StandardizedDevice
            {
                Id = Guid.NewGuid(),
                DateStamp = DateTime.Now,
                Message = CharacteristicToByteArray()
            };
        }

        public byte[] CharacteristicToByteArray()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();

                bf.Serialize(ms, DeviceCharacteristics);
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
