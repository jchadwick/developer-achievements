using DeveloperAchievements.Achievements;

namespace DeveloperAchievements.DataAccess.NHibernate.Mappings
{
    public class AchievementTypeMapping : KeyedEntityMapping<AchievementDescriptor>
    {
        public AchievementTypeMapping()
        {
            Map(x => x.Name);
        }
    }
}
