using DeveloperAchievements.Achievements;

namespace DeveloperAchievements.DataAccess.NHibernate.Mappings
{
    public class AchievementDescriptorMapping : KeyedEntityMapping<AchievementDescriptor>
    {
        public AchievementDescriptorMapping()
        {
            Map(x => x.Name);
            Map(x => x.Description);
        }
    }
}
