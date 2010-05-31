using DeveloperAchievements;
using DeveloperAchievements.DataAccess;

namespace Setup
{
    public class DatabaseBuilder
    {
        private readonly IDataConfiguration _configuration;

        public DatabaseBuilder(IDataConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void DropAndCreateDatabase()
        {
            _configuration.DropDatabase();
            _configuration.CreateDatabase();
        }
    }
}