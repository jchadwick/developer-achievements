using System;
using System.Collections.Generic;
using ChadwickSoftware.DeveloperAchievements.Activities;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using ClientActivity = ChadwickSoftware.DeveloperAchievements.Client.Activity;

namespace ChadwickSoftware.DeveloperAchievements.Client
{
    [TestFixture]
    public class DeveloperActivityServiceClientTests : IntegrationTestFixture
    {
        private DeveloperActivityService _client;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _client = new DeveloperActivityService();
        }

        [Test]
        public void ShouldLogDeveloperActivity()
        {
            ClientActivity activity = new ClientActivity()
            {
                ActivityType = "ChadwickSoftware.DeveloperAchievements.Activities.CheckIn",
                Timestamp = DateTime.Now,
                Username = "jchadwick",
                ActivityParameters = new Dictionary<string, string>()
                                                {
                                                    {"Revision", Guid.NewGuid().ToString()}
                                                },
            };

            LogDeveloperActivityResponse response = _client.LogDeveloperActivity(activity);

            CheckIn checkinActivity = Repository.Get<CheckIn>(response.ActivityID);
            Assert.That(checkinActivity.Revision, Is.EqualTo(activity.ActivityParameters["Revision"]));
        }

        [Test, Explicit("Manual test harness")]
        public void LogSuccessfulBuild()
        {
            ClientActivity activity = new ClientActivity()
            {
                ActivityType = "ChadwickSoftware.DeveloperAchievements.Activities.SuccessfulBuild",
                Timestamp = DateTime.Now,
                Username = "tnuegent",
                ActivityParameters = new Dictionary<string, string>()
                                                {
                                                    {"Url", "http://build1.local/cctray/builds/123456"}
                                                },
            };

            for (int i = 0; i < 5; i++)
            {
                _client.LogDeveloperActivity(activity);
            }
        }
    }
}
