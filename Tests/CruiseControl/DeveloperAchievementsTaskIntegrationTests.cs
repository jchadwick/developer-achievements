using System;
using System.Linq;
using System.Linq.Expressions;
using ChadwickSoftware.DeveloperAchievements.Activities;
using DeveloperAchievements.CruiseControl;
using NUnit.Framework;
using Moq;
using NUnit.Framework.SyntaxHelpers;
using ThoughtWorks.CruiseControl.Core;

namespace ChadwickSoftware.DeveloperAchievements.CruiseControl
{
    [TestFixture]
    public class DeveloperAchievementsTaskIntegrationTests : IntegrationTestFixture
    {
        private const string UserName = "jchadwick";

        private DeveloperAchievementsTask _task;
        private Mock<IIntegrationResult> _mockIntegrationResult;

        [SetUp]
        public void SetUp()
        {
            _mockIntegrationResult = new Mock<IIntegrationResult>();
            _mockIntegrationResult.SetupAllProperties();
            _mockIntegrationResult.SetupGet(x => x.HasSourceControlError).Returns(false);
            _mockIntegrationResult.SetupGet(x => x.Fixed).Returns(false);
            _mockIntegrationResult.SetupGet(x => x.Succeeded).Returns(true);
            _mockIntegrationResult.SetupGet(x => x.Modifications).Returns(new[] { new Modification() { UserName = UserName } });

            _task = new DeveloperAchievementsTask();
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
