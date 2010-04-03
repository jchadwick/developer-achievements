using System;
using System.IO;
using DeveloperAchievements.Subversion.InterOp;
using NUnit.Framework;

namespace DeveloperAchievements.Integration.Subversion.InterOp
{
    [TestFixture]
    public class SvnLookTests
    {
        private const string RepositoryPath = @"C:\Temp\repos";

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            InterOpHelper.ExecutablePath = Path.Combine(Environment.CurrentDirectory, @"..\..\..\References\svn");
        }

        [Test]
        public void ShouldGetSvnInfoFromRepository()
        {
            VerifyTestRepositoryExists();

            var looker = new SvnLook();
            var info = looker.Info(new SvnRevisionParameters(RepositoryPath, "1"));
            Assert.That(info.Message.EndsWith("Creating test repository"));
        }

        [Test]
        [ExpectedException(typeof(ApplicationException))]
        public void ShouldThrowExceptionWhenTryingToGetSvnInfoFromRepositoryWithInvalidInfo()
        {
            VerifyTestRepositoryExists();

            new SvnLook().Info(new SvnRevisionParameters(RepositoryPath, "INVALID_ID"));
        }

        [CoverageExclude("The second half of this is HOPEFULLY never run because the test repository already exists")]
        private void VerifyTestRepositoryExists()
        {
            if (Directory.Exists(RepositoryPath) == false)
                throw new ApplicationException(
                    "Test repository does not exist!  Try running CreateTestSubversionRepository.bat");
        }

    }
}