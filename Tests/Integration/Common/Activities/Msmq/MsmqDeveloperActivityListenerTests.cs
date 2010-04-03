using System;
using System.Threading;
using DeveloperAchievements.Achievements.Msmq;
using DeveloperAchievements.Achievements;
using DeveloperAchievements.Activities;
using DeveloperAchievements.Activities.Msmq;
using Moq;
using NUnit.Framework;

namespace DeveloperAchievements.Integration.Common.Activities.Msmq
{
    [TestFixture]
    public class MsmqDeveloperActivityListenerTests : MsmqTestFixtureBase
    {
        private Mock<IAchievementService> _mockAchievementService;
        private Mock<IDeveloperActivityRepository> _mockDeveloperActivityRepository;
        private MsmqDeveloperActivityListener _listener;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _mockAchievementService = new Mock<IAchievementService>();
            _mockDeveloperActivityRepository = new Mock<IDeveloperActivityRepository>();
            _mockDeveloperActivityRepository
                .Setup(x => x.Save(It.IsAny<DeveloperActivity>()));

            _listener = new MsmqDeveloperActivityListener(_mockAchievementService.Object, _mockDeveloperActivityRepository.Object);
        }

        [Test]
        public void ShouldTriggerEventWhenDeveloperActivityOccurs()
        {
            _mockAchievementService
                .Setup(x => x.GenerateAchievement(It.IsAny<DeveloperActivity>()))
                .Verifiable();

            using (_listener)
            {
                _listener.Start();

                var repository = new MsMqDeveloperActivitySource();
                repository.Save(new CheckIn());
                repository.Save(new CheckIn());

                Thread.Sleep(TimeSpan.FromSeconds(1));
            }

            _mockAchievementService.Verify();
        }

        [Test]
        public void ShouldNotTriggerEventWhenDeveloperActivityOccursIfStopped()
        {
            _listener.Start();
            _listener.Stop();

            var repository = new MsMqDeveloperActivitySource();
            repository.Save(new CheckIn());
            Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        [Test]
        public void ShouldDisposeProperly()
        {
            _listener.Dispose();
        }

    }
}