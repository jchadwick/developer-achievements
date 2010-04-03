using System.Diagnostics;
using System.Messaging;

namespace DeveloperAchievements.Activities.Msmq
{
    public static class MsmqConfiguration
    {
        private static string _queuePath = @".\Private$\DeveloperActivities";
        internal static string QueuePath
        {
            get { return _queuePath; }
            set { _queuePath = value; }
        }

        public static MessageQueue CreateQueue()
        {
            if (MessageQueue.Exists(QueuePath) == false)
            {
                Debug.WriteLine(string.Format("No Message Queue exists for path '{0}' - creating...", QueuePath));
                MessageQueue.Create(QueuePath);
            }

            Debug.WriteLine(string.Format("Returning new instance of Message Queue '{0}'.", QueuePath));
            return new MessageQueue(QueuePath);
        }
    }
}