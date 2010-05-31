using DeveloperAchievements.Activities;

namespace DeveloperAchievements.Achievements
{
    public interface IAchievementGenerator
    {
        /// <summary>
        /// Generates Achievements for a given Developer Activity catalyst.
        /// </summary>
        /// <param name="catalyst">The developer activity that triggered the Achievement generation</param>
        /// <param name="history">The developer's activity history</param>
        /// <returns>An award generation result that contains the generated awards for the catalyst.</returns>
        AchievementGenerationResult Generate(DeveloperActivity catalyst, DeveloperHistory history);
    }

}
