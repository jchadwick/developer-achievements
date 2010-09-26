using System.Collections.Generic;
using System.Linq;
using ChadwickSoftware.DeveloperAchievements.AchievementGeneration;
using ChadwickSoftware.DeveloperAchievements.AchievementGeneration.Calculators;
using ChadwickSoftware.DeveloperAchievements.DataAccess;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using ChadwickSoftware.DeveloperAchievements.Activities;

namespace ChadwickSoftware.DeveloperAchievements.AchievementCalculators
{
    [TestFixture]
    public class DefaultAchievementCalculatorTests
    {
        private IRepository _repository;
        private Developer _developer;
        private IEnumerable<DefaultAchievementCalculator> _calculators;

        [SetUp]
        public void SetUp()
        {
            _repository = new MockRepository();

            _developer = new Developer() { Username = "jchadwick" };

            _calculators = new[] { new DefaultAchievementCalculator(_repository) };

            _repository.Save(_developer);
        }

        [Test]
        public void ShouldAwardAchievementsForAnActivity()
        {
            _repository.Save(new Achievement() { TriggerCount = 1, TargetActivityTypeName = typeof(BrokenBuild).FullName });

            AchievementGenerator generator = new AchievementGenerator(_calculators);

            Activity activity = new BrokenBuild() { Developer = _developer };
            _developer.History.Add(activity);

            generator.GenerateAchievements(activity);

            AwardedAchievement awardedAchievement = _repository.Query<AwardedAchievement>().Single();
            Assert.That(awardedAchievement, Is.Not.Null);
        }

        [Test]
        public void ShouldAwardAccumulatedAchievementsForActivities()
        {
            const int triggerCount = 5;

            _repository.Save(new Achievement() { TriggerCount = triggerCount, TargetActivityTypeName = typeof(BrokenBuild).FullName });

            AchievementGenerator generator = new AchievementGenerator(_calculators);

            // Add enough history so that the next one will trigger the accumulator
            for (int i = 0; i < triggerCount - 1; i++)
            {
                _developer.History.Add(new BrokenBuild() { Developer = _developer });
            }

            Activity activity = new BrokenBuild() { Developer = _developer };
            _developer.History.Add(activity);

            generator.GenerateAchievements(activity);

            AwardedAchievement awardedAchievement = _repository.Query<AwardedAchievement>().Single();
            Assert.That(awardedAchievement, Is.Not.Null);
        }

        [Test]
        public void ShouldNotAwardAccumulatedAchievementsForActivitiesThatDontMeetTheTriggerCount()
        {
            const int triggerCount = 5;

            _repository.Save(new Achievement() { TriggerCount = triggerCount, TargetActivityTypeName = typeof(BrokenBuild).FullName });

            AchievementGenerator generator = new AchievementGenerator(_calculators);

            // No developer history - the next Activity will be the first
            _developer.History.Clear();

            Activity activity = new BrokenBuild() { Developer = _developer };
            _developer.History.Add(activity);

            generator.GenerateAchievements(activity);

            AwardedAchievement awardedAchievement = _repository.Query<AwardedAchievement>().SingleOrDefault();
            Assert.That(awardedAchievement, Is.Null);
        }

    }
}
