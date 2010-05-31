using System.Collections.Generic;
using System.Linq;

namespace DeveloperAchievements.Achievements
{
    public class AchievementGenerationResult
    {
        public IEnumerable<Achievement> GeneratedAchievements { get; set; }

        public bool HasGeneratedAchievements
        {
            get { return GeneratedAchievements != null && GeneratedAchievements.Count() > 0; }
        }

        public AchievementGenerationResult()
        {
            GeneratedAchievements = Enumerable.Empty<Achievement>();
        }
    }
}