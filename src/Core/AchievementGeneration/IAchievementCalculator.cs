using System.Collections.Generic;

namespace ChadwickSoftware.DeveloperAchievements
{
    public interface IAchievementCalculator
    {
        int ExecutionPriority { get; }
        IEnumerable<AwardedAchievement> Calculate(Activity trigger);
    }
}