using System.Threading;

namespace Server.SynchronyHelpers
{
    public class SynchronyHelper
    {
        public SynchronyHelper()
        {
            EventSlim = new ManualResetEventSlim(false);
        }

        public ManualResetEventSlim EventSlim { get; set; }

        private static int Counter { get; set; }

        public void UpdateCounter()
        {
            if (Counter < 5)
            {
                Counter++;
            }
            else
            {
                EventSlim.Set();
            }

        }

    }
}
