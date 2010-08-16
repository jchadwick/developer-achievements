using System.Collections.Generic;

namespace ChadwickSoftware.DeveloperAchievements.Website.Models
{
    public class LeaderBoardStatistics
    {
        public IEnumerable<Developer> RockStars { get; set; }
        public IEnumerable<Developer> n00bs { get; set; }

        public IEnumerable<AchievementGroup> AchievementGroups { get; set; }

        public IEnumerable<ActivityGroup> ActivityGroups { get; set; }
    }
}