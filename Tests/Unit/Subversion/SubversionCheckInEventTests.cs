using System;
using DeveloperAchievements.Achievements;
using DeveloperAchievements.Activities;
using DeveloperAchievements.Subversion;
using DeveloperAchievements.Subversion.InterOp;
using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace DeveloperAchievements.Unit.Subversion
{
    [TestFixture]
    public class SubversionCheckInEventTests
    {

        [Test]
        public void ShouldSaveCheckinInformationToActivityRepository()
        {
            DeveloperActivity actualActivity = null;

            SvnRevisionParameters actualParameters = null;
            var mockSvnLook = new Mock<SvnLook>();
            mockSvnLook
                .Setup(x => x.Info(It.IsAny<SvnRevisionParameters>()))
                .Callback((SvnRevisionParameters x) => actualParameters = x)
                .Returns(() => new SvnLog(actualParameters.RepositoryPath, actualParameters.Identifier, DateTime.Now, "jchadwick", "This is a test checkin"));

            var mockRepository = new Mock<IDeveloperActivityRepository>();
            mockRepository
                .Setup(x => x.Save(It.IsAny<DeveloperActivity>()))
                .Callback((DeveloperActivity a) => actualActivity = a)
                .Verifiable();

            var listener = new ServerSideCommitListener(mockRepository.Object, mockSvnLook.Object);
            listener.OnCommit("/svn/myrepository", "12345");

            mockRepository.Verify();
            Assert.That(actualActivity, Is.TypeOf(typeof(CheckIn)));
            Assert.That(actualActivity.Username, Is.EqualTo("jchadwick"));
        }

    }
}