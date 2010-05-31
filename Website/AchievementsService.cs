using Ninject;

namespace DeveloperAchievements.Website
{
    public class AchievementsService : DeveloperAchievements.Services.AchievementsService
    {
        public AchievementsService()
            : base(Global.Container.Get<IRepository>())
        {
            
        }
    }
}