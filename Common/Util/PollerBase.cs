using System;
using System.Diagnostics;

namespace DeveloperAchievements.Util
{
    public abstract class PollerBase
    {
        protected internal static readonly TimeSpan DefaultPollDelay = TimeSpan.FromSeconds(10);
        private readonly TimeSpan _pollDelay;
        private bool _shouldKeepGoing;


        protected PollerBase() : this(DefaultPollDelay)
        {
        }
        protected PollerBase(TimeSpan pollDelay)
        {
            _pollDelay = pollDelay;
        }


        public void Start()
        {
            if(_shouldKeepGoing)
            {
                return;
            }

            _shouldKeepGoing = true;

            while (_shouldKeepGoing)
            {
                Execute();
                Debug.WriteLine(string.Format("Sleeping for {0}...", _pollDelay));
                SleepHelper.Sleep(_pollDelay);
            }
        }

        public void Stop()
        {
            _shouldKeepGoing = false;
        }

        protected internal abstract void Execute();
    }
}