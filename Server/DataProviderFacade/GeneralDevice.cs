using System;
using System.Collections.Generic;
using System.Text;

namespace DataProviderFacade
{
    public class GeneralDevice
    {
        public int Id { get; set; }

        public DateTime DateStamp { get; set; }

        public byte[] Message { get; set; }

        public string DeviceIdentifier { get; set; }
    }
}
