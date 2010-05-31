using System.Runtime.Serialization;

namespace DeveloperAchievements.Achievements
{
    [DataContract]
    public class Achievement : KeyedEntity
    {

        [DataMember]
        public virtual User User { get; set; }

        [DataMember]
        public virtual AchievementDescriptor AwardedAchievement { get; set; }

    }
}