using DeveloperAchievements.Activities;

namespace DeveloperAchievements.Achievements
{
    public interface IAchievementService
    {
        void Award(string username, AchievementDescriptor achievementDescriptor);
        void Award(Developer developer, AchievementDescriptor achievementDescriptor);
        void GenerateAchievements(DeveloperActivity activity);
    }
}