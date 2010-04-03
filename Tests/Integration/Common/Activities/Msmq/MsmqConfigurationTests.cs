using System.Messaging;
using DeveloperAchievements.Activities.Msmq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace DeveloperAchievements.Integration.Common.Activities.Msmq
{
    [TestFixture]
    public class MsmqConfigurationTests : MsmqTestFixtureBase
    {

        [Test]
        public void ShouldCreateQueueIfQueueDoesntExist()
        {
            if(MessageQueue.Exists(MsmqConfiguration.QueuePath))
                MessageQueue.Delete(MsmqConfiguration.QueuePath);

            MsmqConfiguration.CreateQueue();

            Assert.That(MessageQueue.Exists(MsmqConfiguration.QueuePath), Is.True);
        }

    }
}