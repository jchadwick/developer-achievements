using System;
using System.Diagnostics;
using DeveloperAchievements.Util;

namespace DeveloperAchievements.Activities
{
    public class PollingDeveloperActivityListener : PollerBase, IDeveloperActivityListener
    {
        private readonly IDeveloperActivitySource _activitySource;

        public event EventHandler<DeveloperActivityEventArgs> DeveloperActivityDetected;

        public PollingDeveloperActivityListener(IDeveloperActivitySource activitySource) 
        {
            if(activitySource == null)
                throw new ArgumentNullException("activitySource");

            _activitySource = activitySource;
        }

        protected internal override void Execute()
        {
            DeveloperActivity activity = null;

            try
            {
                Debug.WriteLine("Polling for next unprocessed developer activity...");
                activity = _activitySource.RetrieveNext();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Error listening for developer activity: {0}.\n{1}", ex.Message, ex.StackTrace));
                // Don't rethrow - just keep going.
            }

            try
            {
                if (activity == null)
                {
                    Debug.WriteLine("No unprocessed developer activities found.");
                }
                else
                {
                    Debug.WriteLine("Heard developer activity - triggering event...");
                    if (DeveloperActivityDetected != null)
                        DeveloperActivityDetected(this, new DeveloperActivityEventArgs(activity));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Error saving developer activity: {0}.\n{1}", ex.Message, ex.StackTrace));
                // Don't rethrow - just keep going.
            }
        }

    }
}