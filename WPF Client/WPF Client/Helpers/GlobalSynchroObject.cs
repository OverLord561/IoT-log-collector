namespace WPF_Client.Helpers
{
    public class GlobalSynchroObject
    {
        private bool _isFirstStart = true;
        private readonly static object _locker;

        static GlobalSynchroObject()
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
    }
}
