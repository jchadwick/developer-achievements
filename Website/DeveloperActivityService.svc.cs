using System;
using System.Data.Services;
using System.ServiceModel;
using DeveloperAchievements.DataAccess;
using DeveloperAchievements.DataAccess.NHibernate;

namespace DeveloperAchievements.Website
{
    public class DataServiceBase<T> : DataService<T>
    {
        public DataServiceBase()
        {
            Global.Container.RequestContainer.InjectProperties(this);
        }
    }

    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class DeveloperActivityService : DataServiceBase<DeveloperActivityDataContext>
    {
        public DeveloperActivityDataContext Context { get; set; }

        protected override DeveloperActivityDataContext CreateDataSource()
        {
            return Context;
        }

        public static void InitializeService(DataServiceConfiguration config)
        {
            config.UseVerboseErrors = true;
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
        }
    }
}
