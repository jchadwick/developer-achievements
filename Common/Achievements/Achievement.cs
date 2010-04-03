namespace DeveloperAchievements.Achievements
{
    public class Achievement : KeyedEntity
    {

        public virtual Developer Developer { get; set; }

        public virtual AchievementDescriptor AwardedAchievement { get; set; }

    }
}