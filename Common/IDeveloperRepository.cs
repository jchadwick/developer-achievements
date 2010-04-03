namespace DeveloperAchievements
{
    public interface IDeveloperRepository
    {
        Developer GetOrCreate(string username);
    }
}