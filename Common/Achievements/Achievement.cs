using System.Data.Services.Common;

namespace DeveloperAchievements.Achievements
{
    [DataServiceEntity]
    public class Achievement : KeyedEntity
    {
        public virtual Developer Developer { get; set; }

        public virtual AchievementDescriptor AwardedAchievement { get; set; }

    }
}