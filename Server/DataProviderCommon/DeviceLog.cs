using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataProviderCommon
{
    public class DeviceLog
    {
        [Required]
        public int Id { get; set; }

        public string PluginName { get; set; }

        public DateTime DateStamp { get; set; }

        public byte[] Message { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is DeviceLog another))
                return false;

            return ReferenceEquals(this, another) || (Id == another.Id && PluginName == another.PluginName && DateStamp == another.DateStamp && Message == another.Message);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
