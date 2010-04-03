namespace DeveloperAchievements.Activities
{
    public class TaskActivity : DeveloperActivity
    {
        public virtual string TaskId { get; set; }
        public virtual string Url { get; set; }
        public virtual string Action { get; set; }
    }
}