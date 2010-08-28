using System;
using System.Collections.Generic;
using System.Linq;
using ChadwickSoftware.DeveloperAchievements.DataAccess;

namespace ChadwickSoftware.DeveloperAchievements.AchievementGeneration
{
    public interface IAchievementGenerator
    {
        int GenerateAchievements(Activity trigger);
    }

    public class AchievementGenerator : IAchievementGenerator
    {
        private readonly IRepository _repository;
        private readonly IEnumerable<IAchievementCalculator> _achievementCalculators;

        public AchievementGenerator(IRepository repository, IEnumerable<IAchievementCalculator> achievementCalculators)
        {
            _repository = repository;
            _achievementCalculators = achievementCalculators;
        }

        public int GenerateAchievements(Activity trigger)
        {
            IOrderedEnumerable<IAchievementCalculator> orderedAchievementCalculators =
                _achievementCalculators.OrderBy(x => x.ExecutionPriority);

            IEnumerable<AwardedAchievement> achievements = ExecuteCalculators(orderedAchievementCalculators, trigger);

            _repository.SaveAll(achievements);

            IEnumerable<Developer> developers = achievements.Select(x => x.Developer).Distinct();
            foreach (Developer developer in developers)
            {
                _repository.Refresh(developer);
                UpdateDeveloperStatistics(developer);
            }

            _repository.ExecuteSql(
                @"UPDATE dev SET Rank=rankings.[Rank], RankLastCalculated=getdate()
                  FROM Developers dev
                  LEFT JOIN (SELECT ID, RANK() OVER(ORDER BY Percentage DESC) AS [Rank] 
                             FROM Developers 
                             WHERE Percentage IS NOT NULL) rankings ON dev.ID = rankings.ID");

            return achievements.Count();
        }

        private void UpdateDeveloperStatistics(Developer developer)
        {
            DeveloperStatistics stats = developer.Statistics;

            stats.TotalAchievementsCount = developer.Achievements.Count();
            stats.PositiveAchievementsCount = developer.Achievements.Where(x => x.Achievement.Disposition == AchievementDisposition.Positive).Sum(x => x.Count);
            stats.NegativeAchievementsCount = developer.Achievements.Where(x => x.Achievement.Disposition == AchievementDisposition.Negative).Sum(x => x.Count);
            stats.TotalNonNeutralAchievementsCount = (stats.PositiveAchievementsCount + stats.NegativeAchievementsCount); 

            if (stats.TotalNonNeutralAchievementsCount > 0)
                stats.Percentage = (stats.PositiveAchievementsCount / (decimal)stats.TotalNonNeutralAchievementsCount);

            stats.LastUpdated = DateTime.Now;

            _repository.Save(developer);
        }

        private static IEnumerable<AwardedAchievement> ExecuteCalculators(IEnumerable<IAchievementCalculator> orderedAchievementCalculators, Activity trigger)
        {
            List<AwardedAchievement> awardedAchievements = new List<AwardedAchievement>();

            foreach (var achievementCalculator in orderedAchievementCalculators)
            {
                try
                {
                    awardedAchievements.AddRange(achievementCalculator.Calculate(trigger));
                }
                catch (Exception e)
                {
                    // Swallow any exceptions to avoid letting the other achievements run
                    // TODO: Add logging
                    Console.WriteLine(e);
                }
            }

            return awardedAchievements;
        }
    }
}
