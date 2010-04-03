using DeveloperAchievements.Subversion;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Console=DeveloperAchievements.Subversion.Console;

namespace DeveloperAchievements.Unit.Subversion
{
    [TestFixture]
    public class ConsoleTests
    {

        [Test]
        public void ShouldCallTheCheckinListenerWhenCommitActionOccurs()
        {
            var expectedRepositoryPath = "/svn/myRepos";
            var expectedIdentifier = "12345-i";

            var mockCheckinListener = new MockCheckInListener();
            Console.ServerSideCommitListener = mockCheckinListener;

            Console.Main(new[] { "commit", expectedRepositoryPath, expectedIdentifier });

            Assert.That(mockCheckinListener.OnCommitWasCalled, Is.True);
        }



        class MockCheckInListener : ServerSideCommitListener
        {
            public bool OnCommitWasCalled { get; private set; }

            public MockCheckInListener() : base(null, null) { }

            public override void OnCommit(string repositoryPath, string identifier)
            {
                OnCommitWasCalled = true;
            }
        }

    }
}