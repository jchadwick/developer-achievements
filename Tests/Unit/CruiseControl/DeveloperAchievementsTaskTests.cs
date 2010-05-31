using System.Collections;
using DeveloperAchievements.CruiseControl;
using Moq;
using NUnit.Framework;
using ThoughtWorks.CruiseControl.Core;

namespace DeveloperAchievements.Unit.CruiseControl
{
    [TestFixture]
    public class DeveloperAchievementsTaskTests
    {
        private DeveloperAchievementsTask _task;
        private Mock<IIntegrationResult> _mockResult;
        private Mock<IBuildService> _mockRepository;

        [SetUp]
        public void SetUp()
        {
            _mockResult = new Mock<IIntegrationResult>();
            _mockRepository = new Mock<IBuildService>();
            _task = new DeveloperAchievementsTask(_mockRepository.Object);
        }

        [Test]
        public void ShouldCreateAnAchievementForEveryUserInTheCheckin()
        {
            _mockResult
                .Setup(x => x.FailureUsers)
                .Returns(new ArrayList() { "someuser" });

            _task.Run(_mockResult.Object);

            _mockRepository
                .Verify(x => x.AddBuild("someuser", It.IsAny<string>()));
        }
    }
}
