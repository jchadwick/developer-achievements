using System;

namespace ChadwickSoftware.DeveloperAchievements
{
    public class DeveloperStatistics
    {
        public virtual DateTime? LastUpdated { get; set; }

        public virtual int NegativeAchievementsCount { get; set; }

        public virtual decimal? Percentage { get; set; }

        public virtual int PositiveAchievementsCount { get; set; }

        public virtual int? Rank { get; set; }

        public virtual DateTime? RankLastCalculated { get; set; }

        public virtual int TotalAchievementsCount { get; set; }

        public virtual int TotalNonNeutralAchievementsCount { get; set; }
    }
}