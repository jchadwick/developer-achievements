using System.Linq;
using ChadwickSoftware.DeveloperAchievements.Client;
using DeveloperAchievements.CruiseControl;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Moq;
using ThoughtWorks.CruiseControl.Core;

namespace ChadwickSoftware.DeveloperAchievements.CruiseControl
{
    [TestFixture]
    public class DeveloperBuildActivityTaskTests
    {
        private DeveloperBuildActivityTask _task;
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

            _mockActivityService = new Mock<IDeveloperActivityService>();

            _task = new DeveloperBuildActivityTask(_mockActivityService.Object);
        }

        [Test]
        public void ShouldReportASuccessfulBuild()
        {
            Client.Activity actualLoggedActivity = null;

            _mockActivityService
                .Setup(x => x.LogDeveloperActivity(It.IsAny<Client.Activity>()))
                .Callback((Client.Activity x) => actualLoggedActivity = x);

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
            Client.Activity actualLoggedActivity = null;

            _mockActivityService
                .Setup(x => x.LogDeveloperActivity(It.IsAny<Client.Activity>()))
                .Callback((Client.Activity x) => actualLoggedActivity = x);

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
            Client.Activity actualLoggedActivity = null;

            _mockActivityService
                .Setup(x => x.LogDeveloperActivity(It.IsAny<Client.Activity>()))
                .Callback((Client.Activity x) => actualLoggedActivity = x);

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
            
            Client.Activity actualLoggedActivity = null;

            _mockActivityService
                .Setup(x => x.LogDeveloperActivity(It.IsAny<Client.Activity>()))
                .Callback((Client.Activity x) => actualLoggedActivity = x);

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
                .Verify(x => x.LogDeveloperActivity(It.IsAny<Client.Activity>()),
                        Times.Exactly(modifications.Count()));
        }
    }
}
