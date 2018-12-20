namespace Server.Models
{
    public class UserSettings
    {
        public string DataProviderPluginName { get; set; }

        public int CapacityOfCollectionToInsert { get; set; }

        public int IntervalForWritingIntoDb { get; set; }
    }
}
