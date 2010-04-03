using System;
using DeveloperAchievements.Achievements;
using DeveloperAchievements.Activities;
using DeveloperAchievements.Util;
using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace DeveloperAchievements.Unit.Common.Activities
{
    [TestFixture]
    public class PollingDeveloperActivityListenerTests
    {

        [TearDown]
        public void TearDown()
        {
            SleepHelper.Sleeper = new MockSleeper();
        }


        [Test]
        public void ShouldTriggerEventForRetrievedActivities()
        {
            bool eventWasTriggered = false;
            var expectedActivity = new CheckIn();

            var developerActivitySource = new Mock<IDeveloperActivitySource>();
            developerActivitySource
                .Setup(x => x.RetrieveNext())
                .Returns(expectedActivity);

            var listener = new PollingDeveloperActivityListener(developerActivitySource.Object);
            listener.DeveloperActivityDetected += (sender, args) => { eventWasTriggered = true; };

            listener.Execute();

            Assert.That(eventWasTriggered);
        }

        [Test]
        public void ShouldNotTriggerEventForNullActivity()
        {
            bool eventWasTriggered = false;

            var developerActivitySource = new Mock<IDeveloperActivitySource>();
            developerActivitySource
                .Setup(x => x.RetrieveNext())
                .Returns((DeveloperActivity)null);

            var listener = new PollingDeveloperActivityListener(developerActivitySource.Object);

            listener.DeveloperActivityDetected += (sender, args) => { eventWasTriggered = true; };

            listener.Execute();
            
            Assert.That(eventWasTriggered, Is.Not.True);
        }

        [Test]
        public void ShouldNotBeKilledByAnExceptionFromTheActivityRepository()
        {
            var developerActivitySource = new Mock<IDeveloperActivitySource>();
            developerActivitySource
                .Setup(x => x.RetrieveNext())
                .Throws(new ApplicationException());

            var listener = new PollingDeveloperActivityListener(developerActivitySource.Object);

            // Stop after the first sleep call
            var sleeper = new MockSleeper(x => listener.Stop());
            SleepHelper.Sleeper = sleeper;

            listener.Start();

            Assert.That(sleeper.SleepCount, Is.GreaterThanOrEqualTo(1));
        }

    }
}