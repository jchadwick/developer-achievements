using DeveloperAchievements.Achievements;
using FluentNHibernate.Mapping;

namespace DeveloperAchievements.DataAccess.NHibernate.Mappings.Activities
{
    public class BuildMapping : SubclassMap<Build>
    {
        public BuildMapping()
        {
            Map(x => x.ReportUrl, "Build_ReportUrl");
            Map(x => x.Result, "Build_Result");
        }
    }
}
