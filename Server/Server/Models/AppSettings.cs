namespace Server.Models
{
    public class AppSettings
    {
        public string ConnectionString { get; set; }

        public UserSettings UserSettings { get; set; }

        public Logging Logging { get; set; }
    }

    public class Logging {
        public bool IncludeScopes { get; set; }

        public LogLevel LogLevel { get; set; }
    }

    public class LogLevel {
        public string Default { get; set; }
        public string System { get; set; }
        public string Microsoft { get; set; }
    }
}
