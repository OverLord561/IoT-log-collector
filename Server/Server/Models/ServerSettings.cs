using Server.ViewModels;

namespace Server.Models
{
    public class ServerSettings
    {
        public ServerSettingViewModel DataStoragePlugin { get; set; }

        public ServerSettingViewModel CapacityOfCollectionToInsert { get; set; }

        public ServerSettingViewModel IntervalForWritingIntoDb { get; set; }
    }
}
