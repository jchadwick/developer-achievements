using System.Linq;
using DeveloperAchievements.CruiseControl;
using NUnit.Framework;
using Moq;
using NUnit.Framework.SyntaxHelpers;
using ThoughtWorks.CruiseControl.Core;

namespace ChadwickSoftware.DeveloperAchievements.CruiseControl
{
    [TestFixture]
    public class DeveloperBuildActivityTaskIntegrationTests : IntegrationTestFixture
    {
        private const string UserName = "jchadwick";

        private DeveloperBuildActivityTask _task;
        private Mock<IIntegrationResult> _mockIntegrationResult;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _mockIntegrationResult = new Mock<IIntegrationResult>();
            _mockIntegrationResult.SetupAllProperties();
            _mockIntegrationResult.SetupGet(x => x.HasSourceControlError).Returns(false);
            _mockIntegrationResult.SetupGet(x => x.Fixed).Returns(false);
            _mockIntegrationResult.SetupGet(x => x.Succeeded).Returns(true);
            _mockIntegrationResult.SetupGet(x => x.Modifications).Returns(new[] { new Modification() { UserName = UserName } });

            _task = new DeveloperBuildActivityTask();
        }

        [Test]
        public void ShouldLogAnActivity()
        {
            Developer developer = Repository.Get<Developer>(UserName);
            int startCount = (developer == null) ? 0 : developer.Activities.Count();
            
            _task.Run(_mockIntegrationResult.Object);

            if(developer == null)
                developer = Repository.Get<Developer>(UserName);
            else
                Repository.Refresh(developer);

            int endCount = developer.Activities.Count();
            Assert.That(endCount, Is.EqualTo(startCount + 1));
        }
    }
}
