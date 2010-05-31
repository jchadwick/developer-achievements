using System.IO;
using System.Net;

namespace DeveloperAchievements.CruiseControl
{
    public interface IBuildService
    {
        void AddBuild(string username, string buildResult);
    }

    public class ODataBuildService : IBuildService
    {
        private const string ServiceUrl = "http://localhost:13141/DeveloperActivityService.svc/";
        private readonly string _serviceUrl;

        public ODataBuildService()
            : this(ServiceUrl)
        {
            
        }
        public ODataBuildService(string serviceUrl)
        {
            _serviceUrl = serviceUrl;
        }

        public void AddBuild(string username, string buildResult)
        {
            var request = WebRequest.Create(_serviceUrl + "/Builds");

            request.Method = "POST";
            request.ContentType = "application/json";

            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write("{{ Username: '{0}', Result: '{1}' }}", username, buildResult);
            }
        }

    }
}