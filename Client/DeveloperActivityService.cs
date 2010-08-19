using System.Configuration;
using System.ServiceModel;

namespace ChadwickSoftware.DeveloperAchievements.Client
{
    public class DeveloperActivityService : IDeveloperActivityService
    {
        private const string WebServiceUrlSettingKey = "DeveloperActivityService.Url";
        
        private readonly IDeveloperActivityService _serviceClient;


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


        public LogDeveloperActivityResponse LogDeveloperActivity(Activity activityContract)
        {
            LogDeveloperActivityResponse response = _serviceClient.LogDeveloperActivity(activityContract);
            return response;
        }

        protected virtual IDeveloperActivityService CreateDeveloperActivityServiceClient(string webServiceUrl)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            return new DeveloperActivityServiceClient(binding, new EndpointAddress(webServiceUrl));
        }
    }
}
