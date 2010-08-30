using System.Collections.Generic;
using System.Linq;
using ChadwickSoftware.DeveloperAchievements.DataAccess;

namespace ChadwickSoftware.DeveloperAchievements.AchievementGeneration.Statistics
{
    public interface IStatisticsGenerator
    {
        void UpdateStatistics();
        void UpdateStatistics(Developer developer);
        void UpdateRankings();
    }

    public class StatisticsGenerator : IStatisticsGenerator
    {
        private readonly IRepository _repository;

        public StatisticsGenerator(IRepository repository)
        {
            _repository = repository;
        }

        public void UpdateStatistics()
        {
            IEnumerable<Developer> developers = _repository.Query<Developer>();

            foreach (var developer in developers)
            {
                UpdateStatistics(developer);
            }
        }

        public void UpdateStatistics(Developer developer)
        {
            PopulateStatistics(developer, developer.Statistics);
        }

        internal void PopulateStatistics(Developer developer, DeveloperStatistics statistics)
        {
            statistics.TotalAchievementsCount = developer.Achievements.Count();
            statistics.PositiveAchievementsCount = developer.Achievements.Where(x => x.Achievement.Disposition == AchievementDisposition.Positive).Sum(x => x.Count);
            statistics.NegativeAchievementsCount = developer.Achievements.Where(x => x.Achievement.Disposition == AchievementDisposition.Negative).Sum(x => x.Count);
            statistics.TotalNonNeutralAchievementsCount = (statistics.PositiveAchievementsCount + statistics.NegativeAchievementsCount); 

            if (statistics.TotalNonNeutralAchievementsCount > 0)
                statistics.Percentage = (statistics.PositiveAchievementsCount / (decimal)statistics.TotalNonNeutralAchievementsCount);
        }

        public void UpdateRankings()
        {
            _repository.ExecuteSql(
                @"UPDATE dev SET Rank=rankings.[Rank], RankLastCalculated=getdate()
                  FROM Developers dev
                  LEFT JOIN (SELECT ID, RANK() OVER(ORDER BY Percentage DESC) AS [Rank] 
                             FROM Developers 
                             WHERE Percentage IS NOT NULL) rankings ON dev.ID = rankings.ID");
        }
    }
}
