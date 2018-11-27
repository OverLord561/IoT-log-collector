using System;
using System.Collections.Generic;
using System.Text;

namespace DataProviderCommon
{
    [Serializable]
    public class StandardizedDevice
    {
        public Guid Id { get; set; }

        public DateTime DateStamp { get; set; }

        public byte[] Message { get; set; }
    }
}
