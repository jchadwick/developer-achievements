using System;
using System.Threading;

namespace DeveloperAchievements.Util
{
    public static class SleepHelper
    {
        private static readonly object SleeperLock = new object();
        private static ISleeper _sleeper;
        static internal ISleeper Sleeper
        {
            get
            {
                if(_sleeper == null)
                {
                    lock (SleeperLock)
                    {
                        if (_sleeper == null)
                            _sleeper = new RealSleeper();
                    }
                }

                return _sleeper;
            }
            set { _sleeper = value; }
        }

        public static void Sleep(int timeToSleepInMilliseconds)
        {
            Sleeper.Sleep(timeToSleepInMilliseconds);
        }

        public static void Sleep(TimeSpan timeToSleep)
        {
            Sleeper.Sleep(timeToSleep);
        }

    }

    internal interface ISleeper
    {
        void Sleep(int milliseconds);
    }

    internal static class ISleeperExtensions
    {
        public static void Sleep(this ISleeper sleeper, TimeSpan timeSpan)
        {
            sleeper.Sleep((int)timeSpan.TotalMilliseconds);
        }
    }

    internal class RealSleeper : ISleeper
    {
        public void Sleep(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }
    }

    internal class MockSleeper : ISleeper
    {
        public Action<int> OnSleepCallback { get; set; }

        public int SleepCount { get; set; }

        public double LastSleepTimeInMilliseconds { get; set; }


        public MockSleeper() : this(null)
        {
        }
        public MockSleeper(Action<int> callback)
        {
            OnSleepCallback = callback;
            SleepCount = 0;
        }


        public void Sleep(int milliseconds)
        {
            LastSleepTimeInMilliseconds = milliseconds;
            SleepCount++;

            if(OnSleepCallback != null)
                OnSleepCallback.Invoke(milliseconds);
        }
    }
}