using System;
using System.Linq;
using ChadwickSoftware.DeveloperAchievements.AchievementGeneration;
using ChadwickSoftware.DeveloperAchievements.Activities;
using Moq;
using NHibernate;
using NHibernate.Exceptions;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace ChadwickSoftware.DeveloperAchievements.DataAccess
{
    [TestFixture]
    public class NHibernateRepositoryTests : IntegrationTestFixture
    {
        private NHibernateConfiguration _config;
        private ISessionFactory _sessionFactory;
        private ISession _session;
        private NHibernateRepository _repository;
        private Developer _developer;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _config = new MsSqlNHibernateConfiguration();
            _config.CreateDatabase();

            _sessionFactory = _config.CreateSessionFactory();
            _session = _sessionFactory.OpenSession();
            _repository = new NHibernateRepository(_session);

            _developer = new Developer() {Username = "jchadwick", DisplayName = "Jess Chadwick"};
            _repository.Save(_developer);
        }

        [TearDown]
        public void TearDown()
        {
            if (_session != null)
                _session.Dispose();
        }

        [Test]
        public void ShouldSaveDeveloperHistory()
        {
            _developer.History.Add(new CheckIn() { Revision = "1" });
            _developer.History.Add(new BrokenBuild() { Url = "http://build1/builds/1"});
            _developer.History.Add(new CheckIn() { Revision = "2" });
            _developer.History.Add(new SuccessfulBuild() { Url = "http://build1/builds/2" });
            _developer.History.Add(new CheckIn() { Revision = "3" });
            _developer.History.Add(new SuccessfulBuild() { Url = "http://build1/builds/3" });
            _developer.History.Add(new CheckIn() { Revision = "4" });
            _developer.History.Add(new BrokenBuild() { Url = "http://build1/builds/4" });
            _developer.History.Add(new CheckIn() { Revision = "5" });
            _developer.History.Add(new SuccessfulBuild() { Url = "http://build1/builds/5" });
            _developer.History.Add(new CheckIn() { Revision = "6" });
            _developer.History.Add(new BrokenBuild() { Url = "http://build1/builds/6" });

            _repository.Save(_developer);
            _session.Flush();
        }

        [Test]
        public void ShouldSaveAwardedAchievements()
        {
            int previousDeveloperAchievementCount = _developer.Achievements.Count();

            Achievement achievement = new Achievement()
                                  {
                                      Name = "Bob the Builder",
                                      Disposition = AchievementDisposition.Positive,
                                      Key = "bobthebuilder",
                                      Kind = AchievementKind.Medal,
                                      TriggerCount = 1,
                                      TargetActivityTypeName = typeof(SuccessfulBuild).FullName
                                  };

            _repository.Save(achievement);
            _session.Flush();

            DateTime lastAwardedTimestamp = DateTime.Now;
            _repository.Save(new AwardedAchievement() { Achievement = achievement, Count = 1, Developer = _developer, LastAwardedTimestamp = lastAwardedTimestamp });
            _session.Flush();

            _session.Refresh(_developer);
            Assert.That(_developer.Achievements.Count(), Is.EqualTo(previousDeveloperAchievementCount + 1));
        }

        [Test]
        public void ShouldAwardABunchOfAchievementsAndThenUpdateRankings()
        {
            Achievement positiveAchievement = new Achievement()
            {
                Name = "Bob the Builder",
                Disposition = AchievementDisposition.Positive,
                Key = "bobthebuilder",
                Kind = AchievementKind.Medal,
                TriggerCount = 1,
                TargetActivityTypeName = typeof(SuccessfulBuild).FullName
            };

            Achievement negativeAchievement = new Achievement()
            {
                Name = "Bob the Breaker",
                Disposition = AchievementDisposition.Negative,
                Key = "bobthebreaker",
                Kind = AchievementKind.Medal,
                TriggerCount = 1,
                TargetActivityTypeName = typeof(BrokenBuild).FullName
            };

            _repository.Save(positiveAchievement);
            _repository.Save(negativeAchievement);

            Random random = new Random();
            for (int i = 0; i < 30; i++)
            {
                Developer developer = new Developer() {Username = "Developer #" + i};

                for (int j = 0; j < random.Next(i); j++)
                    developer.Activities.Add(new CheckIn() { Developer = developer, Revision = Guid.NewGuid().ToString() });

                for (int j = 0; j < random.Next(i); j++)
                    developer.Achievements.Add(new AwardedAchievement() { Achievement = positiveAchievement, Developer = developer, Count = 1 });

                for (int j = 0; j < random.Next(i); j++)
                    developer.Achievements.Add(new AwardedAchievement() { Achievement = negativeAchievement, Developer = developer, Count = 1 });
                
                _repository.Save(developer);
            }

            _session.Flush();

            RegenerateRankings();
        }

        private void RegenerateRankings()
        {
            Mock<IAchievementCalculator> mockCalculator = new Mock<IAchievementCalculator>();
            mockCalculator
                .Setup(x => x.Calculate(It.IsAny<Activity>()))
                .Returns(_repository.Query<AwardedAchievement>());

            AchievementGenerator generator = new AchievementGenerator(new[] { mockCalculator.Object });
            generator.GenerateAchievements(new SuccessfulBuild() {Developer = _developer});
        }


        [Test, ExpectedException(typeof(GenericADOException))]
        public void ShouldThrowExceptionWhenTryingToSaveDuplicateKey()
        {
            Developer newDeveloper = new Developer() {Key = _developer.Key};
            _repository.Save(newDeveloper);
        }

    }
}
