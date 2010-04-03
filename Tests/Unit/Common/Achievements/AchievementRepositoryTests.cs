using DeveloperAchievements.Achievements;
using Moq;
using NUnit.Framework;

namespace DeveloperAchievements.Unit.Common.Achievements
{
    [TestFixture]
    public class AchievementRepositoryTests
    {

        [Test]
        public void ShouldSaveAchievementToBaseRepository()
        {
            var award = new Achievement();

            var baseRepository = new Mock<IRepository>();
            baseRepository
                .Setup(x => x.Save(award))
                .Verifiable();

            new AchievementRepository(baseRepository.Object).Save(award);

            baseRepository.VerifyAll();
        }

    }
}