using System;

namespace DeveloperAchievements.Activities
{

    public class DeveloperActivityEventArgs : EventArgs
    {

        public DeveloperActivity Activity { get; set; }

        public DeveloperActivityEventArgs(DeveloperActivity activity)
        {
            Activity = activity;
        }

    }

    public interface IDeveloperActivityListener
    {

        event EventHandler<DeveloperActivityEventArgs> DeveloperActivityDetected;

    }

}