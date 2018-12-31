using System.Configuration;

namespace WPF_Client.Helpers
{
    public class GlobalObject
    {
        private bool _isFirstStart = true;
        private readonly static object _locker;

        static GlobalObject()
        {
            _locker = new object();
        }

        public bool IsIFirstStart
        {
            get
            {
                lock (_locker)
                {
                    return _isFirstStart;
                }
            }

            set
            {
                lock (_locker)
                {
                    _isFirstStart = value;
                }
            }
        }

        public string GetChartDataUrl()
        {
            //TODO ADD TimeStamp and DevicePlugin Name from server settings
            var reletiveUrl = string.Format(ConfigurationManager.AppSettings["ServerUrl"], "", IsIFirstStart, "SamsungDPlugin");
            return reletiveUrl;
        }
    }
}
