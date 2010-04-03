using System.Messaging;
using DeveloperAchievements.Activities.Msmq;
using NUnit.Framework;

namespace DeveloperAchievements.Integration.Common.Activities.Msmq
{
    public class MsmqTestFixtureBase
    {
        protected MessageQueue Queue;

        [TestFixtureSetUp]
        public virtual void TestFixtureSetUp()
        {
            if (MsmqConfiguration.QueuePath.EndsWith("_IntegrationTesting") == false)
                MsmqConfiguration.QueuePath += "_IntegrationTesting";
        }

        [SetUp]
        public virtual void SetUp()
        {
            Queue = MsmqConfiguration.CreateQueue();
            Queue.Purge();
        }

        [TearDown]
        public virtual void TearDown()
        {
            Queue.Purge();
            Queue.Dispose();
        }
    }
}