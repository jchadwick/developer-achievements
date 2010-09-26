using System.Linq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Moq;
using ThoughtWorks.CruiseControl.Core;
using ThoughtWorks.CruiseControl.Core.Util;
using ChadwickSoftware.DeveloperAchievements.CruiseControl.Proxy;
using ProxyActivity = ChadwickSoftware.DeveloperAchievements.CruiseControl.Proxy.Activity;

namespace ChadwickSoftware.DeveloperAchievements.CruiseControl
{
    [TestFixture]
    public class DeveloperBuildActivityTaskTests
    {
        private DeveloperAchievementsPluginTask _task;
        private Mock<IDeveloperActivityService> _mockActivityService;
        private Mock<IIntegrationResult> _mockIntegrationResult;

        [SetUp]
        public void SetUp()
        {
            _mockIntegrationResult = new Mock<IIntegrationResult>();
            _mockIntegrationResult.SetupAllProperties();
            _mockIntegrationResult.SetupGet(x => x.HasSourceControlError).Returns(false);
            _mockIntegrationResult.SetupGet(x => x.Fixed).Returns(false);
            _mockIntegrationResult.SetupGet(x => x.Succeeded).Returns(false);
            _mockIntegrationResult.SetupGet(x => x.Modifications).Returns(new [] { new Modification() {UserName = "jchadwick"} });
            _mockIntegrationResult.SetupGet(x => x.BuildProgressInformation).Returns(new BuildProgressInformation(string.Empty, string.Empty));

            _mockActivityService = new Mock<IDeveloperActivityService>();

            _task = new DeveloperAchievementsPluginTask();
        }

        [Test]
        public void ShouldReportASuccessfulBuild()
        {
            ProxyActivity actualLoggedActivity = null;

            _mockActivityService
                .Setup(x => x.LogDeveloperActivities(It.IsAny<LogDeveloperActivityRequest>()))
                .Callback((LogDeveloperActivityRequest x) => actualLoggedActivity = x.Activities[0]);

            _mockIntegrationResult.SetupGet(x => x.Succeeded).Returns(true);

            _task.Run(_mockIntegrationResult.Object);

            Assert.That(actualLoggedActivity, Is.Not.Null,
                        "No Activity was logged!");

            Assert.That(actualLoggedActivity.ActivityType.EndsWith("SuccessfulBuild"),
                        "Expected a SuccessfulBuild, but got " + actualLoggedActivity.ActivityType);
        }

        [Test]
        public void ShouldReportABrokenBuild()
        {
            ProxyActivity actualLoggedActivity = null;

            _mockActivityService
                .Setup(x => x.LogDeveloperActivities(It.IsAny<LogDeveloperActivityRequest>()))
                .Callback((LogDeveloperActivityRequest x) => actualLoggedActivity = x.Activities[0]);

            _mockIntegrationResult.SetupGet(x => x.Succeeded).Returns(false);

            _task.Run(_mockIntegrationResult.Object);

            Assert.That(actualLoggedActivity, Is.Not.Null,
                        "No Activity was logged!");

            Assert.That(actualLoggedActivity.ActivityType.EndsWith("BrokenBuild"),
                        "Expected a BrokenBuild, but got " + actualLoggedActivity.ActivityType);
        }

        [Test]
        public void ShouldReportAFixedBuild()
        {
            ProxyActivity actualLoggedActivity = null;

            _mockActivityService
                .Setup(x => x.LogDeveloperActivities(It.IsAny<LogDeveloperActivityRequest>()))
                .Callback((LogDeveloperActivityRequest x) => actualLoggedActivity = x.Activities[0]);

            _mockIntegrationResult.SetupGet(x => x.Succeeded).Returns(true);
            _mockIntegrationResult.SetupGet(x => x.Fixed).Returns(true);

            _task.Run(_mockIntegrationResult.Object);

            Assert.That(actualLoggedActivity, Is.Not.Null,
                        "No Activity was logged!");

            Assert.That(actualLoggedActivity.ActivityType.EndsWith("FixedBuild"),
                        "Expected a FixedBuild, but got " + actualLoggedActivity.ActivityType);
        }

        [Test]
        public void ShouldSetTheBuildUrl()
        {
            const string expectedUrl = "http://build-server/12345.aspx";

            ProxyActivity actualLoggedActivity = null;

            _mockActivityService
                .Setup(x => x.LogDeveloperActivities(It.IsAny<LogDeveloperActivityRequest>()))
                .Callback((LogDeveloperActivityRequest x) => actualLoggedActivity = x.Activities[0]);

            _mockIntegrationResult.SetupGet(x => x.Succeeded).Returns(true);
            _mockIntegrationResult.SetupGet(x => x.ProjectUrl).Returns(expectedUrl);

            _task.Run(_mockIntegrationResult.Object);

            Assert.That(actualLoggedActivity, Is.Not.Null,
                        "No Activity was logged!");

            Assert.That(actualLoggedActivity.ActivityParameters["Url"], Is.EqualTo(expectedUrl));
        }

        [Test]
        public void ShouldLogAnActivityForEachUserInvolvedInABuild()
        {
            Modification[] modifications = new[]
                                               {
                                                   new Modification() {UserName = "fsinatra"},
                                                   new Modification() {UserName = "hsimpson"},
                                                   new Modification() {UserName = "morphius"},
                                               };

            _mockIntegrationResult.SetupGet(x => x.Succeeded).Returns(true);
            _mockIntegrationResult.SetupGet(x => x.Modifications).Returns(modifications);

            _task.Run(_mockIntegrationResult.Object);

            _mockActivityService
                .Verify(x => x.LogDeveloperActivities(It.IsAny<LogDeveloperActivityRequest>()),
                        Times.Exactly(modifications.Count()));
        }
    }
}
