using DeveloperAchievements.CruiseControl;
using NUnit.Framework;

namespace DeveloperAchievements.Integration.CruiseControl
{
    [TestFixture]
    public class ODataBuildServiceTests
    {
        [Test]
        public void ShouldAddNewBuild()
        {
            var buildService = new ODataBuildService();
            buildService.AddBuild("jchadwick", "Success");
        }
    }
}
