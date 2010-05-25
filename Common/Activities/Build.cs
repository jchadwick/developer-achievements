using DeveloperAchievements.Activities;

namespace DeveloperAchievements.Achievements
{
    public enum BuildResult : short
    {
        Failed = -1,
        Unknown = 0,
        Success = 1,
        Fixed = 2
    }

    public class Build : DeveloperActivity
    {
        public virtual string ReportUrl { get; set; }

        public virtual BuildResult Result { get; set; }
    }

}