using System.Collections.Generic;

namespace ChadwickSoftware.DeveloperAchievements.Website.Models
{
    public class AchievementGroup
    {
        public string Name
        {
            get { return Achievement.Name; }
        }

        public Achievement Achievement { get; set; }

        public IEnumerable<AwardedAchievement> TopAwards { get; set; }
    }
}