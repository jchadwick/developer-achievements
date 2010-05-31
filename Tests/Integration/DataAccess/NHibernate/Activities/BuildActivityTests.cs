using DeveloperAchievements.Activities;
using NUnit.Framework;

namespace DeveloperAchievements.Integration.DataAccess.NHibernate.Activities
{
    [TestFixture]
    public class BuildActivityTests : RepositoryTestBase
    {

        [Test]
        public void ShouldSaveBuildActivity()
        {
            Repository.Save(new Build
                                {
                                    Username = "jchadwick",
                                    ReportUrl = "http://build.test.com/reports/1234.html",
                                    Result = BuildResult.Success
                                });
        }

    }
}