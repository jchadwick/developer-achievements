using System;
using System.Collections.Generic;
using System.Linq;
using ChadwickSoftware.DeveloperAchievements.Client;
using ThoughtWorks.CruiseControl.Core;
using ThoughtWorks.CruiseControl.Remote;

namespace DeveloperAchievements.CruiseControl
{
    public class DeveloperAchievementsTask : ITask
    {
        private readonly IDeveloperActivityService _activityService;

        public DeveloperAchievementsTask()
            : this(new DeveloperActivityService())
        {
        }

        public DeveloperAchievementsTask(IDeveloperActivityService activityService)
        {
            _activityService = activityService;
        }

        public void Run(IIntegrationResult result)
        {
            bool shouldIgnoreBuild = 
                   result.BuildCondition == BuildCondition.ForceBuild 
                || result.Status == IntegrationStatus.Cancelled 
                || result.Status == IntegrationStatus.Unknown;

            if (shouldIgnoreBuild) return;

            IEnumerable<string> usernames = result.Modifications.Select(m => m.UserName).Distinct();

            string activityTypeName = GetActivityTypeName(result);

            Dictionary<string, string> parameters = 
                new Dictionary<string, string>
                    {
                        { "Url", result.ProjectUrl },
                    };

            IEnumerable<Activity> activities =
                from username in (usernames ?? Enumerable.Empty<string>())
                select new Activity()
                           {
                               ActivityType = activityTypeName,
                               ActivityParameters = parameters,
                               Timestamp = DateTime.Now,
                               Username = username,
                           };

            foreach (Activity activity in activities)
                _activityService.LogDeveloperActivity(activity);
        }

        private static string GetActivityTypeName(IIntegrationResult result)
        {
            string activityName;

            if(result.Fixed)
                activityName = "Fixed";

            else if(result.Succeeded)
                activityName = "Successful";

            else
                activityName = "Broken";

            return string.Format("ChadwickSoftware.DeveloperAchievements.Activities.{0}Build", activityName);
        }
    }
}
