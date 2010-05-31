using System.Runtime.Serialization;

namespace DeveloperAchievements.Achievements
{
    [DataContract]
    public class Achievement : KeyedEntity
    {

        [DataMember]
        public virtual Developer Developer { get; set; }

        [DataMember]
        public virtual AchievementDescriptor AwardedAchievement { get; set; }

    }
}