using System.Runtime.Serialization;

namespace DeveloperAchievements.Activities
{
    [DataContract]
    public class TaskActivity : DeveloperActivity
    {

        [DataMember]
        public virtual string TaskId { get; set; }
        
        [DataMember]
        public virtual string Url { get; set; }

        [DataMember]
        public virtual string Action { get; set; }

    }
}