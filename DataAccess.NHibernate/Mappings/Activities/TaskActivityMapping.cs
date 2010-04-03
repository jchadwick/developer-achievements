using DeveloperAchievements.Activities;
using FluentNHibernate.Mapping;

namespace DeveloperAchievements.DataAccess.NHibernate.Mappings.Activities
{
    public class TaskActivityMapping : SubclassMap<TaskActivity>
    {
        public TaskActivityMapping()
        {
            Map(x => x.TaskId, "TaskActivity_TaskId");
            Map(x => x.Action, "TaskActivity_Action");
            Map(x => x.Url, "TaskActivity_Url");
        }
    }
}
