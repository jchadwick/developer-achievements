using System.Configuration;
using System.ServiceModel;

// ReSharper disable CheckNamespace
namespace ChadwickSoftware.DeveloperAchievements.Client
{
    /// <summary>
    /// Facade on top of the WCF service client intended to provide WCF ignorance.
    /// Feel free to use this or create your own Service Reference - same thing.
    /// </summary>
    public class DeveloperActivityService : IDeveloperActivityService
    {
        private const string WebServiceUrlSettingKey = "DeveloperActivityService.Url";
        
        private readonly IDeveloperActivityService _serviceClient;


        // ReSharper disable DoNotCallOverridableMethodsInConstructor
        public DeveloperActivityService()
        {
            string webServiceUrl = ConfigurationManager.AppSettings[WebServiceUrlSettingKey];
            _serviceClient = CreateDeveloperActivityServiceClient(webServiceUrl);
        }

        public DeveloperActivityService(string webServiceUrl)
        {
            _serviceClient = CreateDeveloperActivityServiceClient(webServiceUrl);
        }

        public DeveloperActivityService(IDeveloperActivityService serviceClient)
        {
            _serviceClient = serviceClient;
        }
        // ReSharper restore DoNotCallOverridableMethodsInConstructor


        public LogDeveloperActivityResponse LogDeveloperActivities(LogDeveloperActivityRequest request)
        {
            LogDeveloperActivityResponse response = _serviceClient.LogDeveloperActivities(request);
            return response;
        }

        protected virtual IDeveloperActivityService CreateDeveloperActivityServiceClient(string webServiceUrl)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            return new DeveloperActivityServiceClient(binding, new EndpointAddress(webServiceUrl));
        }
    }
}
// ReSharper restore CheckNamespace
