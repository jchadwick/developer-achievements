using System.Messaging;
using DeveloperAchievements.Achievements.Msmq;

namespace DeveloperAchievements.Activities.Msmq
{
    public class MsMqDeveloperActivitySource : IDeveloperActivitySource
    {
        private readonly MsmqDeveloperActivitySerializer _serializer;

        public MsMqDeveloperActivitySource()
            : this(new MsmqDeveloperActivitySerializer())
        {
        }
        public MsMqDeveloperActivitySource(MsmqDeveloperActivitySerializer serializer)
        {
            _serializer = serializer;
        }

        public void Save(DeveloperActivity activity)
        {
            var label = string.Format("{0}: {1}, {2}", activity.GetType().Name, activity.Username, activity.CreatedTimeStamp);

            Message message = new Message { Label = label, };
            _serializer.Serialize(activity, message);

            using (MessageQueue queue = MsmqConfiguration.CreateQueue())
                queue.Send(message);
        }

        public DeveloperActivity RetrieveNext()
        {
            using (MessageQueue queue = MsmqConfiguration.CreateQueue())
            {
                var message = queue.Receive();
                var deserializedObject = _serializer.Deserialize(message);
                return deserializedObject;
            }
        }
    }
}