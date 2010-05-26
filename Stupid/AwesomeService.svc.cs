using System.Data.Services;
using System.ServiceModel;

namespace Stupid
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class AwesomeService : DataService<AwesomeDataContext>
    {
        public static void InitializeService(DataServiceConfiguration config)
        {
            config.UseVerboseErrors = true;
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
        }
    }
}
