using System.Web.Services;
using Ninject;
using Ninject.Extensions.Logging;

namespace ChadwickSoftware.DeveloperAchievements.Website.Services
{
    public class WebServiceBase : WebService
    {
        [Inject]
        protected ILogger Logger { get; set; }

        public WebServiceBase()
            : this(MvcApplication.Kernel)
        {
        }

        public WebServiceBase(IKernel kernel)
        {
            if (kernel != null)
                kernel.Inject(this);
        }
    }
}