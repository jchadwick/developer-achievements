using ChadwickSoftware.DeveloperAchievements.AchievementGeneration;
using ChadwickSoftware.DeveloperAchievements.AchievementGeneration.Calculators;
using ChadwickSoftware.DeveloperAchievements.AchievementGeneration.Statistics;
using Ninject.Modules;

namespace ChadwickSoftware.DeveloperAchievements
{
    public class CoreBindingModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IAchievementGenerator>().To<AchievementGenerator>();
            Bind<IAchievementCalculator>().To<DefaultAchievementCalculator>();
            Bind<IStatisticsGenerator>().To<StatisticsGenerator>();
        }
    }
}
