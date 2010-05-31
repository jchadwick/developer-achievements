using System.Data.Services;
using System.ServiceModel;
using DeveloperAchievements.DataAccess;
using Ninject;

namespace DeveloperAchievements.Website
{

    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class DeveloperActivityService : DataService<DeveloperActivityDataContext>
    {
        protected override DeveloperActivityDataContext CreateDataSource()
        {
            return Global.Container.Get<DeveloperActivityDataContext>();
        }

        public static void InitializeService(DataServiceConfiguration config)
        {
            config.UseVerboseErrors = true;
            config.SetEntitySetAccessRule("*", EntitySetRights.AllRead);
        }
    }

}
