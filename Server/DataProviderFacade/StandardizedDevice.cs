using System;
using System.Collections.Generic;
using System.Text;

namespace DataProviderFacade
{
    [Serializable]
    public class StandardizedDevice
    {
        public Guid Id { get; set; }

        public DateTime DateStamp { get; set; }

        public byte[] Message { get; set; }
    }
}
