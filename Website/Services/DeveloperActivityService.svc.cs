using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web.Compilation;
using ChadwickSoftware.DeveloperAchievements.AchievementGeneration;
using ChadwickSoftware.DeveloperAchievements.Website.Services.Contracts;
using Ninject;
using Ninject.Web;

namespace ChadwickSoftware.DeveloperAchievements.Website.Services
{
    [ServiceContract]
    public interface IDeveloperActivityService
    {
        [OperationContract]
        LogDeveloperActivityResponse LogDeveloperActivity(ActivityContract activityContract);
    }

    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DeveloperActivityService : WebServiceBase, IDeveloperActivityService
    {
        [Inject]
        public IRepository Repository { get; set; }

        [Inject]
        public IAchievementGenerator AchievementGenerator { get; set; }


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
            int generatedAchievements = AchievementGenerator.GenerateAchievements(activity);
            Logger.Debug("{0} achievements generated", generatedAchievements);

            Logger.Info("Done logging developer activity.  Developer ID: {0}; Activity ID: {1}; ActivityType: [{2}]; Timestamp: {3}",
                        developer.ID, activity.ID, activityTypeInstance.FullName, timestamp);

            return new LogDeveloperActivityResponse()
                       {
                           ActivityID = activity.ID,
                           AwardedAchievementCount = generatedAchievements,
                       };
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
