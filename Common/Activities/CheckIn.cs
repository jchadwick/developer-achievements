using System.Runtime.Serialization;

namespace DeveloperAchievements.Activities
{
    [DataContract]
    public class CheckIn : DeveloperActivity
    {
        [DataMember]
        public virtual string RepositoryPath { get; set; }

        [DataMember]
        public virtual int Revision { get; set; }

        [DataMember]
        public virtual string Message { get; set; }
    }
}
