using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using DeveloperAchievements.Achievements;
using DeveloperAchievements.Activities;

namespace DeveloperAchievements.Services
{
    [ServiceContract]
    public interface IAchievementsService
    {
        // /achievments/{name}
        [OperationContract]
        [WebGet(UriTemplate = "achievements", 
                BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<AchievementDescriptor> GetAchievements();

        [OperationContract]
        [WebGet(UriTemplate = "achievements/{name}", 
                BodyStyle = WebMessageBodyStyle.Wrapped)]
        IEnumerable<AchievementDescriptor> GetAchievementByName(string name);

        // /activities/{id}
        [OperationContract]
        [WebGet(UriTemplate = "activities/{key}", ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<DeveloperActivity> GetActivities(string key);
        
        // /users/{username}
        [OperationContract]
        [WebGet(UriTemplate = "users/{username}", ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<User> GetUsers(string username);

        // /users/{username}/achievements
        [OperationContract]
        [WebGet(UriTemplate = "users/{username}/achievements", ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<Achievement> GetUserAchievements(string username);

        // /users/{username}/activities/{id}
        [OperationContract]
        [WebGet(UriTemplate = "users/{username}/activities/{id}", ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<DeveloperActivity> GetUserActivities(string username, string id);

        [OperationContract]
        [WebInvoke(UriTemplate = "users/{username}/activities", ResponseFormat=WebMessageFormat.Json)]
        string AddUserActivity(string username, DeveloperActivity activity);

    }

    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AchievementsService : IAchievementsService
    {
        public IRepository Repository { get; private set; }

        public AchievementsService()
        {
        }

        public AchievementsService(IRepository repository)
        {
            Repository = repository;
        }


        #region Implementation of IAchievementsService

        public IEnumerable<AchievementDescriptor> GetAchievements(string name)
        {
            var achievements = Repository.FindAll<AchievementDescriptor>();
            return achievements;
        }

        public IEnumerable<AchievementDescriptor> GetAchievements()
        {
            var achievements = Repository.FindAll<AchievementDescriptor>();
            return achievements;
        }

        public IEnumerable<AchievementDescriptor> GetAchievementByName(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DeveloperActivity> GetActivities(string id)
        {
            return Repository.Query<DeveloperActivity>(x => x.Key == id);
        }

        public IEnumerable<User> GetUsers(string username)
        {
            return Repository.Query<User>(x => x.Username == username);
        }

        public IEnumerable<Achievement> GetUserAchievements(string username)
        {
            return Repository.Query<Achievement>(x => x.User.Username == username);
        }

        public IEnumerable<DeveloperActivity> GetUserActivities(string username, string key)
        {
            return (string.IsNullOrEmpty(key))
                ? Repository.Query<DeveloperActivity>(x => x.Key == username) 
                : GetActivities(key);
        }

        public string AddUserActivity(string username, DeveloperActivity activity)
        {
            activity.Username = username;
            Repository.Save(activity);

            return activity.Key;
        }

        #endregion
    }

}
