using DataProviderFacade;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TemperatureController
{
    [Serializable]
    public class Samsung_RT38F : IStandardizedDeviceOperations
    {
        static BinaryFormatter bf;
        private StandardizedDevice standardizedDevice;

        private const string GuidId = "CE9A8BDB-1E95-4517-B97C-754262401CB3";
        private const string Name = "Samsung_RT38F";
        private decimal CelciusTemperature { get; set; }

        static Samsung_RT38F()
        {
            bf = new BinaryFormatter();
        }

        public Samsung_RT38F(decimal celciusTemperature)
        {
            this.CelciusTemperature = celciusTemperature;

            standardizedDevice = ConverterToStandard();
        }

        // Interface methods
        public StandardizedDevice ConverterToStandard()
        {
            return new StandardizedDevice
            {
                Id = Guid.NewGuid(),
                DateStamp = DateTime.Now,
                Message = ObjectToByteArray()
            };
        }

        public byte[] ObjectToByteArray()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, this);
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
