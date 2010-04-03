using DeveloperAchievements.Achievements;
using DeveloperAchievements.Activities;
using DeveloperAchievements.DataAccess.NHibernate;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace DeveloperAchievements.Integration.DataAccess.NHibernate
{
    [TestFixture]
    public class RepositoryTests : RepositoryTestBase
    {

        [Test]
        public void ShouldSaveNewAchievement()
        {
            long awardId;
            Achievement expectedAchievment;

            using(var repository = new Repository(SessionFactory.OpenSession()))
            {
                var developer = repository.FindByKey<Developer>("jchadwick") ?? new Developer() { Username = "jchadwick" };
                var awardType = repository.FindByKey<AchievementDescriptor>("build+breaker") ?? new AchievementDescriptor() { Name = "Build Breaker" };
                expectedAchievment = new Achievement() { Developer = developer, AwardedAchievement = awardType };

                repository.Save(developer);
                repository.Save(awardType);
                repository.Save(expectedAchievment);
                awardId = expectedAchievment.Id;
            }

            using(var repository = new Repository(SessionFactory.OpenSession()))
            {
                var actualAchievement = repository.Find<Achievement>(awardId);
                Assert.That(actualAchievement, Is.EqualTo(expectedAchievment));
            }
        }

        [Test]
        public void ShouldSaveNewDeveloperHistory()
        {
            long historyId;
            DeveloperHistory history;

            using (var repository = new Repository(SessionFactory.OpenSession()))
            {
                var tempHistory = repository.FindByKey<DeveloperHistory>("jchadwick");
                if(tempHistory != null)
                    repository.Delete(tempHistory);

                var developer = repository.FindByKey<Developer>("jchadwick") ?? new Developer() { Username = "jchadwick" };
                history = new DeveloperHistory() { Developer = developer };

                repository.Save(developer);
                repository.Save(history);
                historyId = history.Id;
            }

            using (var repository = new Repository(SessionFactory.OpenSession()))
            {
                var actualHistory = repository.Find<DeveloperHistory>(historyId);
                Assert.That(actualHistory, Is.EqualTo(history));
            }
        }

    }
}