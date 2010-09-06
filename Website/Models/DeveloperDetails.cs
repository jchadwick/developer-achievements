using System.Collections.Generic;
using System.Linq;

namespace ChadwickSoftware.DeveloperAchievements.Website.Models
{
    public class DeveloperDetails
    {

        public string DisplayName { get; set; }


        public int ActivityCount { get; set; }

        public IEnumerable<Activity> Activities { get; set; }


        public int? NeutralAchievementCount { get; set; }

        public IEnumerable<AwardedAchievement> NeutralAchievements { get; set; }


        public int? NegativeAchievementCount { get; set; }

        public IEnumerable<AwardedAchievement> NegativeAchievements { get; set; }


        public int? PositiveAchievementCount { get; set; }

        public IEnumerable<AwardedAchievement> PositiveAchievements { get; set; }



        public DeveloperDetails()
        {
            PositiveAchievements = Enumerable.Empty<AwardedAchievement>();
            NegativeAchievements = Enumerable.Empty<AwardedAchievement>();
        }
    }
}