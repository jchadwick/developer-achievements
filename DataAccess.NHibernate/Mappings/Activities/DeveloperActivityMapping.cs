using DeveloperAchievements.Activities;

namespace DeveloperAchievements.DataAccess.NHibernate.Mappings.Activities
{
    public class DeveloperActivityMapping : KeyedEntityMapping<DeveloperActivity>
    {
        public const string SchemaName = "[Activities]";

        public DeveloperActivityMapping()
        {
            Schema(SchemaName);
            Map(x => x.Username);
            Map(x => x.Processed);

            DiscriminateSubClassesOnColumn("ActivityType");
        }
    }
}