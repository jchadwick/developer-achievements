using ChadwickSoftware.DeveloperAchievements.Activities;
using ChadwickSoftware.DeveloperAchievements.Website.Services.Contracts;
using Ninject;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using LogDeveloperActivityResponse = ChadwickSoftware.DeveloperAchievements.Website.Services.Contracts.LogDeveloperActivityResponse;

namespace ChadwickSoftware.DeveloperAchievements.Website.Services
{
    [TestFixture]
    public class DeveloperActivityServiceTests : IntegrationTestFixture
    {
        [Inject]
        public DeveloperActivityService Service { get; set; }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [Test]
        public void ShouldLogAnActivity()
        {
            ActivityContract request = new ActivityContract()
                                           {
                                               ActivityType = "SuccessfulBuild",
                                               Username = "jchadwick",
                                           };

            LogDeveloperActivityResponse response = Service.LogDeveloperActivity(request);

            Activity activity = Repository.Get<Activity>(response.ActivityResults[0].Activity);
            Assert.That(activity, Is.Not.Null);
        }
    }
}
