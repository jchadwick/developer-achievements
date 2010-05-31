using System.Collections;
using System.Linq;
using ThoughtWorks.CruiseControl.Core;

namespace DeveloperAchievements.CruiseControl
{
    public class DeveloperAchievementsTask : ITask
    {
        private readonly IBuildService _buildService;

        public DeveloperAchievementsTask(IBuildService buildService)
        {
            _buildService = buildService;
        }

        public void Run(IIntegrationResult result)
        {
            bool shouldIgnoreBuild = result.HasSourceControlError;

            if (shouldIgnoreBuild) return;

            string resultStatus =
                result.Fixed ? "Fixed" 
                : (result.Succeeded ? "Success" : "Failed");

            var buildEvents =
                from username in (result.FailureUsers ?? new ArrayList()).Cast<string>()
                select new {username, result = resultStatus};

            foreach (var build in buildEvents)
                _buildService.AddBuild(build.username, build.result);
        }

    }
}
