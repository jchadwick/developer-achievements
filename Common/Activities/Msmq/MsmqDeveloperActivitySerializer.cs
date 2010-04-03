using System.Messaging;
using System.Runtime.Serialization;
using System.Xml;
using DeveloperAchievements.Activities;

namespace DeveloperAchievements.Achievements.Msmq
{
    public class MsmqDeveloperActivitySerializer
    {
        private readonly DataContractSerializer _serializer;

        public MsmqDeveloperActivitySerializer()
        {
            _serializer = new DataContractSerializer(typeof(DeveloperActivity));
        }

        public void Serialize(DeveloperActivity activity, Message message)
        {
            _serializer.WriteObject(message.BodyStream, activity);
        }

        public DeveloperActivity Deserialize(Message message)
        {
            var deserializedObject = _serializer.ReadObject(XmlReader.Create(message.BodyStream), true);
            return deserializedObject as DeveloperActivity;
        }
    }
}