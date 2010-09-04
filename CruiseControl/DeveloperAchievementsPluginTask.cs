using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using ChadwickSoftware.DeveloperAchievements.CruiseControl.Proxy;
using Exortech.NetReflector;
using ThoughtWorks.CruiseControl.Core;
using ThoughtWorks.CruiseControl.Core.Util;
using ThoughtWorks.CruiseControl.Remote;

namespace ChadwickSoftware.DeveloperAchievements.CruiseControl
{

    [ReflectorType("DeveloperAchievementsPlugin")]
    public class DeveloperAchievementsPluginTask : ITask
    {
        [ReflectorProperty("ActivityServiceUrl")]
        public string ActivityServiceUrl;

        public DeveloperActivityServiceClient ActivityService
        {
            get
            {
                _activityService = _activityService ?? CreateDeveloperActivityServiceClient(ActivityServiceUrl);
                return _activityService;
            }
            set { _activityService = value; }
        }
        private DeveloperActivityServiceClient _activityService;


        public void Run(IIntegrationResult result)
        {
            result.BuildProgressInformation.SignalStartRunTask("Generating Achievements");

            bool shouldIgnoreBuild = 
                   result.BuildCondition == BuildCondition.ForceBuild 
                || result.Status == IntegrationStatus.Cancelled 
                || result.Status == IntegrationStatus.Unknown;

            if (shouldIgnoreBuild)
            {
                Log.Info("This build isn't Achievement-worthy - ignoring... (Condition: {0}; Status: {1})", 
                         result.BuildCondition, result.Status);
                return;
            }

            IEnumerable<string> usernames = result.Modifications.Select(m => m.UserName).Distinct();

            Log.Debug("{0} developers involved in this build: {1}", 
                      usernames.Count(), string.Join(", ", usernames.ToArray()));

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


            LogDeveloperActivityRequest request = 
                new LogDeveloperActivityRequest() { Activities = activities.ToList() };

            Log.Debug("Calling the Activity Service ({0}) with a request including {1} {2} activities...",
                      ActivityServiceUrl, activities.Count(), activityTypeName);

            LogDeveloperActivityResponse response = ActivityService.LogDeveloperActivities(request);

            AchievementResult achievementResult = new AchievementResult(response);

            Log.Info("Successfully logged {0} Activities for {1} developers",
                     achievementResult.ActivityResults.Count(), usernames.Count());

            result.AddTaskResult(achievementResult);
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

        protected virtual DeveloperActivityServiceClient CreateDeveloperActivityServiceClient(string webServiceUrl)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            return new DeveloperActivityServiceClient(binding, new EndpointAddress(webServiceUrl));
        }
    }

}
