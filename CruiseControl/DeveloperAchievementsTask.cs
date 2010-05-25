using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DeveloperAchievements.Achievements;
using DeveloperAchievements.Activities;
using DeveloperAchievements.DataAccess.NHibernate;
using ThoughtWorks.CruiseControl.Core;

namespace DeveloperAchievements.CruiseControl
{
    public class DeveloperAchievementsTask : ITask
    {
        private readonly IDeveloperActivityRepository _activityRepository;

        public DeveloperAchievementsTask()
            : this(new DeveloperActivityRepository(Repository.CreateForSingleUse()))
        {
        }

        public DeveloperAchievementsTask(IDeveloperActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        public void Run(IIntegrationResult result)
        {
            bool shouldIgnoreBuild = result.HasSourceControlError;

            if (shouldIgnoreBuild) return;

            BuildResult resultStatus =
                result.Fixed ? BuildResult.Fixed 
                : (result.Succeeded ? BuildResult.Success : BuildResult.Failed);

            IEnumerable<Build> buildEvents =
                from username in (result.FailureUsers ?? new ArrayList()).Cast<string>()
                select new Build()
                            {
                                CreatedTimeStamp = DateTime.Now,
                                Key = username + result.ProjectName + DateTime.Now.ToString("G"),
                                ReportUrl = result.ProjectUrl,
                                Result = resultStatus,
                                Username = username
                       };

            _activityRepository.Save(buildEvents);
        }
    }
}
