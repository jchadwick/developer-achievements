using ChadwickSoftware.DeveloperAchievements.DataAccess;
using Ninject;
using NUnit.Framework;

namespace ChadwickSoftware.DeveloperAchievements
{
    public class IntegrationTestFixture
    {
        [Inject]
        public IRepository Repository { get; set; }

        [SetUp]
        public virtual void SetUp()
        {
            NinjectSettings settings = new NinjectSettings()
                                           {
                                               InjectNonPublic = true
                                           };
            IKernel kernel = new StandardKernel(settings, new CoreBindingModule(), new DataAccessBindingModule());
            kernel.Inject(this);
        }
    }
}
