using DeveloperAchievements.Achievements;
using DeveloperAchievements.Activities;
using FluentNHibernate.Mapping;

namespace DeveloperAchievements.DataAccess.NHibernate.Mappings.Activities
{
    public class CheckinMapping : SubclassMap<CheckIn>
    {
        public CheckinMapping()
        {
            Map(x => x.Revision, "Checkin_Revision");
            Map(x => x.Message, "Checkin_Message");
            Map(x => x.RepositoryPath, "Checkin_RepositoryPath");
        }
    }
}