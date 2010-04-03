using DeveloperAchievements.Activities;

namespace DeveloperAchievements.DataAccess.NHibernate.Mappings.Activities
{
    public class DeveloperHistoryMapping : KeyedEntityMapping<DeveloperHistory>
    {
        public DeveloperHistoryMapping()
        {
            HasOne(x => x.Developer);

            Map(x => x.TotalBuilds);
            Map(x => x.TotalCheckins);
            Map(x => x.TotalTaskActivities);

            HasMany(x => x.PreviousActivity);
        }
    }
}
