using ChadwickSoftware.DeveloperAchievements.Activities;
using FluentNHibernate.Mapping;

namespace ChadwickSoftware.DeveloperAchievements.DataAccess.Mappings
{
    public class ActivityMap : EntityMap<Activity>
    {
        public ActivityMap()
        {
            HasOne(x => x.Developer);

            Map(x => x.Timestamp)
                .Default("getdate()");
            
            DiscriminateSubClassesOnColumn("Type");
        }
    }

    public class BrokenBuildActivityMap : SubclassMap<BrokenBuild>
    {
        public BrokenBuildActivityMap()
        {
            Map(x => x.Url);
        }
    }

    public class SuccessfulBuildActivityMap : SubclassMap<SuccessfulBuild>
    {
        public SuccessfulBuildActivityMap()
        {
            Map(x => x.Url);
        }
    }

    public class CheckInActivityMap : SubclassMap<CheckIn>
    {
        public CheckInActivityMap()
        {
            Map(x => x.Revision);
        }
    }
}
