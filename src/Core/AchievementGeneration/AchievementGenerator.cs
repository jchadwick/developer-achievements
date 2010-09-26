using System;
using System.Collections.Generic;
using System.Linq;

namespace ChadwickSoftware.DeveloperAchievements.AchievementGeneration
{
    public interface IAchievementGenerator
    {
        IEnumerable<AwardedAchievement> GenerateAchievements(Activity trigger);
    }

    public class AchievementGenerator : IAchievementGenerator
    {
        private readonly IEnumerable<IAchievementCalculator> _achievementCalculators;

        public AchievementGenerator(IEnumerable<IAchievementCalculator> achievementCalculators)
        {
            _achievementCalculators = achievementCalculators;
        }

        public IEnumerable<AwardedAchievement> GenerateAchievements(Activity trigger)
        {
            IOrderedEnumerable<IAchievementCalculator> orderedAchievementCalculators =
                _achievementCalculators.OrderBy(x => x.ExecutionPriority);

            IEnumerable<AwardedAchievement> achievements = ExecuteCalculators(orderedAchievementCalculators, trigger);

            return achievements;
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
