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
            IKernel kernel = new StandardKernel(new CoreBindingModule(), new DataAccessBindingModule());
            kernel.Inject(this);
        }
    }
}
