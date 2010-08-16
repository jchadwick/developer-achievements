using ChadwickSoftware.DeveloperAchievements.AchievementCalculators;
using ChadwickSoftware.DeveloperAchievements.AchievementGeneration;
using Ninject.Modules;

namespace ChadwickSoftware.DeveloperAchievements
{
    public class CoreBindingModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IAchievementGenerator>().To<AchievementGenerator>();
            Bind<IAchievementCalculator>().To<DefaultAchievementCalculator>();
        }
    }
}
