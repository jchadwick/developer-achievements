namespace DeveloperAchievements.DataAccess
{
    public interface IDataConfiguration
    {
        void CreateDatabase();
        void DropDatabase();
    }
}
