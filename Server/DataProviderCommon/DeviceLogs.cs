using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataProviderCommon
{
    public class DeviceLogs
    {
        [Required]
        public int Id { get; set; }

        public string DeviceGuid { get; set; }

        public DateTime DateStamp { get; set; }

        public byte[] Message { get; set; }
    }
}
