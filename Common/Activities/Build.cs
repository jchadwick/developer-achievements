using DeveloperAchievements.Activities;

namespace DeveloperAchievements.Achievements
{
    public enum BuildResult : short
    {
        Success = 1,
        Failed = -1
    }

    public class Build : DeveloperActivity
    {
        public virtual string ReportUrl { get; set; }

        public virtual BuildResult Result { get; set; }
    }

}