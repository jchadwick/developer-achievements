using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web.Compilation;
using ChadwickSoftware.DeveloperAchievements.AchievementGeneration;
using ChadwickSoftware.DeveloperAchievements.AchievementGeneration.Statistics;
using ChadwickSoftware.DeveloperAchievements.DataAccess;
using ChadwickSoftware.DeveloperAchievements.Website.Services.Contracts;
using Ninject;

namespace ChadwickSoftware.DeveloperAchievements.Website.Services
{
    [ServiceContract]
    public interface IDeveloperActivityService
    {
        [OperationContract]
        LogDeveloperActivityResponse LogDeveloperActivities(LogDeveloperActivityRequest request);
    }

    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DeveloperActivityService : WebServiceBase, IDeveloperActivityService
    {
        [Inject]
        public IRepository Repository { get; set; }

        [Inject]
        public IAchievementGenerator AchievementGenerator { get; set; }

        [Inject]
        public IStatisticsGenerator StatisticsGenerator { get; set; }


        [WebGet]
        public LogDeveloperActivityResponse LogDeveloperActivities(LogDeveloperActivityRequest request)
        {
            List<ActivityResult> results = new List<ActivityResult>();

            foreach (var activity in request.Activities)
            {
                LogDeveloperActivityResponse response = LogDeveloperActivity(activity);
                results.AddRange(response.ActivityResults);
            }

            return new LogDeveloperActivityResponse(results);
        }

        [WebGet]
        public LogDeveloperActivityResponse LogDeveloperActivity(string username, string activityType, DateTime timestamp)
        {
            ActivityContract contract = new ActivityContract()
                                          {
                                              ActivityType = activityType,
                                              Timestamp = timestamp,
                                              Username = username,
                                          };

            return LogDeveloperActivity(contract);
        }

        [WebGet]
        public LogDeveloperActivityResponse LogDeveloperActivity(ActivityContract activityContract)
        {
            string username = activityContract.Username;
            string activityType = activityContract.ActivityType;
            DateTime timestamp = activityContract.Timestamp.GetValueOrDefault(DateTime.Now);

            Logger.Info("LogDeveloperActivity called for Username: {0}; ActivityType: {1}; Timestamp: {2}", 
                        username, activityType, timestamp);

            Type activityTypeInstance = BuildManager.GetType(activityType, false, true);
            if(activityTypeInstance == null || !typeof(Activity).IsAssignableFrom(activityTypeInstance))
            {
                const string messageFormat = "The provided activityType ({0}) is not a known Activity type.";
                Logger.Error(messageFormat, activityType);
                throw new ApplicationException(string.Format(messageFormat, activityType));
            }

            Developer developer = Repository.Get<Developer>(username);
            if (developer == null)
            {
                developer = new Developer() { Username = username };
                Logger.Debug("Developer username {0} does not currently exist - created new user (ID #{1})", username, developer.ID);
            }

            Activity activity;
            try
            {
                activity = (Activity)activityTypeInstance.GetConstructor(Type.EmptyTypes).Invoke(new object[] {});
                activity.Timestamp = timestamp;
                activity.Developer = developer;
            }
            catch (Exception e)
            {
                const string messageFormat = "Unable to create a new instance of {0}.  Perhaps it doesn't have a default constructor or something?";
                Logger.Error(e, messageFormat, activityType);
                throw new ApplicationException(string.Format(messageFormat, activityType), e);
            }

            try
            {
                ApplyActivityParameters(activity, activityContract.ActivityParameters);
            }
            catch (Exception e)
            {
                const string messageFormat = "Unable to populate instance of {0} from parameters provided. Maybe you have a property name wrong?";
                Logger.Error(e, messageFormat, activityType);
                throw new ApplicationException(string.Format(messageFormat, activityType), e);
            }

            // Add and save the history
            developer.History.Add(activity);
            Repository.Save(developer);

            Logger.Debug("Activity added to {0}'s history.", username);

            // Generate any possible achievements
            IEnumerable<AwardedAchievement> generatedAchievements = GenerateAchievements(activity);

            // Update the developers' statistics based on this new information
            IEnumerable<ActivityResult> activityResults = ProcessAchievements(activity, generatedAchievements);

            return new LogDeveloperActivityResponse(activityResults);
        }

        private IEnumerable<AwardedAchievement> GenerateAchievements(Activity activity)
        {
            IEnumerable<AwardedAchievement> generatedAchievements = AchievementGenerator.GenerateAchievements(activity);
            Repository.SaveAll(generatedAchievements);
            Logger.Debug("{0} achievements generated", generatedAchievements.Count());
            return generatedAchievements;
        }

        private IEnumerable<ActivityResult> ProcessAchievements(Activity activity, IEnumerable<AwardedAchievement> achievements)
        {
            IEnumerable<Developer> developers = achievements.Select(x => x.Developer).Distinct();

            if(developers.Count() > 1)
                Logger.Info("Activity triggered Achievements for more than the current developer.  The following developers were affected: ",
                            string.Join(", ", developers.Select(x => x.Key).ToArray()));

            foreach (Developer developer in developers)
            {
                Logger.Debug("Updating {0}'s statistics...", developer.Key);

                Repository.Refresh(developer);
                StatisticsGenerator.UpdateStatistics(developer);
                Repository.Save(developer);
                Logger.Info("Updated developer statistics", achievements.Count());

                StatisticsGenerator.UpdateRankings();
                Logger.Info("Updated developer rankings", achievements.Count());

                Logger.Info(
                    "Done logging developer activity.  Developer ID: {0}; Activity ID: {1}; ActivityType: [{2}]; Timestamp: {3}",
                    developer.ID, activity.ID, activity.GetType().FullName, activity.Timestamp);

                yield return new ActivityResult()
                                 {
                                     Activity = activity.Key,
                                     Developer = developer.Key,
                                     AwardedAchievementCount = achievements.Count(),
                                     AwardedAchievements = achievements.Select(x => x.Key).ToArray(),
                                 };
            }
        }


        private static void ApplyActivityParameters(Activity activity, IDictionary<string, string> activityParameters)
        {
            foreach (string key in activityParameters.Keys)
            {
                string value = activityParameters[key];
                Type activityType = activity.GetType();
                PropertyInfo property = activityType.GetProperty(key);
                property.SetValue(activity, value, new object[] {});
            }
        }
    }
}
