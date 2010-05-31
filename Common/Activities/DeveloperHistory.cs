using System.Runtime.Serialization;

namespace DeveloperAchievements.Activities
{
    [DataContract]
    public class DeveloperHistory : KeyedEntity
    {
        [DataMember]
        public virtual User User { get; protected internal set; }

        [DataMember]
        public virtual int TotalBuilds { get; protected internal set; }

        [DataMember]
        public virtual int TotalCheckins { get; protected internal set; }

        [DataMember]
        public virtual int TotalTaskActivities { get; protected internal set; }

//        public virtual IEnumerable<DeveloperActivity> PreviousActivity { get; protected internal set; }

        protected override string CreateKey()
        {
            return User.Key;
        }
    }
}
