using DataProviderCommon;
using System;

namespace SamsungTemperatureControllerPlugin
{
    [Serializable]
    public class DeviceCharacteristics
    {
        public double Temperature { get; set; }

        public double Humidity { get; set; }
    }
}
