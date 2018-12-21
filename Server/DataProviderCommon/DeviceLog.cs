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
            var another = obj as DeviceLog;
            if (another == null)
                return false;

            return ReferenceEquals(this, another) || (Id == another.Id && PluginName == another.PluginName && DateStamp == another.DateStamp && Message == another.Message);
        }

    }
}
