using System;
using System.Runtime.Serialization;
using System.Xml;
using DeveloperAchievements.Achievements;
using DeveloperAchievements.Activities.Msmq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace DeveloperAchievements.Integration.Common.Activities.Msmq
{
    [TestFixture]
    public class MsMqDeveloperActivityRepositoryTests : MsmqTestFixtureBase
    {
        [Test]
        public void ShouldSaveActivityToTheMessageQueue()
        {
            var expectedActivity = new CheckIn() { Username = "jchadwick", CreatedTimeStamp = DateTime.Now };

            var repository = new MsMqDeveloperActivitySource();
            repository.Save(expectedActivity);

            var message = Queue.Receive();
            var deserializedObject = new DataContractSerializer(typeof(CheckIn))
                .ReadObject(XmlReader.Create(message.BodyStream), true);
            var actualActivity = (CheckIn)deserializedObject;
            Assert.That(actualActivity.Username, Is.EqualTo(expectedActivity.Username));
            Assert.That(actualActivity.CreatedTimeStamp, Is.EqualTo(expectedActivity.CreatedTimeStamp));
        }

        [Test]
        public void ShouldShouldRetrieveActivityFromTheMessageQueue()
        {
            var expectedActivity = new CheckIn() { Username = "jchadwick", CreatedTimeStamp = DateTime.Now };

            var repository = new MsMqDeveloperActivitySource();
            repository.Save(expectedActivity);

            var actualActivity = repository.RetrieveNext();
            Assert.That(actualActivity.Username, Is.EqualTo(expectedActivity.Username));
            Assert.That(actualActivity.CreatedTimeStamp, Is.EqualTo(expectedActivity.CreatedTimeStamp));
        }

    }
}