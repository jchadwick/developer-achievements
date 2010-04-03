using System.Collections.Generic;
using DeveloperAchievements.Achievements;
using DeveloperAchievements.Activities;
using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace DeveloperAchievements.Unit.Common.Achievements
{
    [TestFixture]
    public class AchievementServiceTests
    {
        private AchievementService _achievementService;
        private Mock<IAchievementRepository> _mockAchievementRepository;
        private Mock<IDeveloperRepository> _mockUserRepository;

        [SetUp]
        public void Setup()
        {
            _mockAchievementRepository = new Mock<IAchievementRepository>();
            _mockUserRepository = new Mock<IDeveloperRepository>();
            _achievementService = new AchievementService(_mockAchievementRepository.Object, _mockUserRepository.Object);
        }

        [Test]
        public void ShouldSaveAnAchievementForAUser()
        {
            Achievement actualAchievment = null;
            _mockAchievementRepository
                .Setup(x => x.Save(It.IsAny<Achievement>()))
                .Callback((Achievement award) => actualAchievment = award);

            _achievementService.Award(new Developer() {Username = "jchadwick"}, new AchievementDescriptor {Name = "Build Breaker"});

            Assert.That(actualAchievment.Developer.Username, Is.EqualTo("jchadwick"));
            Assert.That(actualAchievment.AwardedAchievement.Name, Is.EqualTo("Build Breaker"));
        }

        [Test]
        public void ShouldSaveAnAchievementForAUserByUsername()
        {
            Achievement actualAchievment = null;

            _mockUserRepository
                .Setup(x => x.GetOrCreate("jchadwick"))
                .Returns(new Developer() { Username = "jchadwick" });
            _mockAchievementRepository
                .Setup(x => x.Save(It.IsAny<Achievement>()))
                .Callback((Achievement award) => actualAchievment = award);

            _achievementService.Award("jchadwick", new AchievementDescriptor { Name = "Build Breaker" });

            Assert.That(actualAchievment.Developer.Username, Is.EqualTo("jchadwick"));
            Assert.That(actualAchievment.AwardedAchievement.Name, Is.EqualTo("Build Breaker"));
        }

        [Test]
        public void ShouldCallAchievementGeneratorsWhenTriggeredToDoSoByDeveloperActivity()
        {
            var mockAchievementGenerator = new Mock<IAchievementGenerator>();
            mockAchievementGenerator
                .Setup(x => x.Generate(It.IsAny<DeveloperActivity>(), It.IsAny<DeveloperHistory>()))
                .Returns(new AchievementGenerationResult() { GeneratedAchievements = new[] { new Achievement() } })
                .Verifiable();

            _achievementService.RegisterGenerators(mockAchievementGenerator.Object);
            _achievementService.GenerateAchievements(new CheckIn() { Username = "jchadwick" });

            mockAchievementGenerator.Verify();
        }

        [Test]
        public void ShouldSaveGeneratedAchievements()
        {
            var expectedAchievement = new Achievement();

            var mockAchievementGenerator = new Mock<IAchievementGenerator>();
            mockAchievementGenerator
                .Setup(x => x.Generate(It.IsAny<DeveloperActivity>(), It.IsAny<DeveloperHistory>()))
                .Returns(new AchievementGenerationResult() {GeneratedAchievements = new[] {expectedAchievement}});

            _mockAchievementRepository
                .Setup(x => x.Save(expectedAchievement))
                .Verifiable();

            _achievementService.RegisterGenerators(mockAchievementGenerator.Object);
            _achievementService.GenerateAchievements(new CheckIn() { Username = "jchadwick" });

            _mockAchievementRepository.Verify();
        }

        [Test]
        public void ShouldSaveNothingWhenThereWereNoAchievementsGenerated()
        {
            var mockAchievementGenerator = new Mock<IAchievementGenerator>();
            mockAchievementGenerator
                .Setup(x => x.Generate(It.IsAny<DeveloperActivity>(), It.IsAny<DeveloperHistory>()))
                .Returns(new AchievementGenerationResult());

            _achievementService.RegisterGenerators(mockAchievementGenerator.Object);
            _achievementService.GenerateAchievements(new CheckIn() { Username = "jchadwick" });

            _mockAchievementRepository.Verify(x => x.Save(It.IsAny<Achievement>()), Times.Never(), "Save() was called for a null Achievement");
        }

    }

}