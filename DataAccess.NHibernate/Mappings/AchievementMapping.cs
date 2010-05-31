using DeveloperAchievements.Achievements;

namespace DeveloperAchievements.DataAccess.NHibernate.Mappings
{
    public class AchievementMapping : KeyedEntityMapping<Achievement>
    {
        public AchievementMapping()
        {
            References(x => x.User, "DeveloperID");
            References(x => x.AwardedAchievement, "AchievementTypeID");
        }
    }
}
