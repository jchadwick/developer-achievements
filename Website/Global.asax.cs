using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ChadwickSoftware.DeveloperAchievements.DataAccess;
using Microsoft.WebPages.Compilation;
using Ninject;
using Ninject.Web.Mvc;

namespace ChadwickSoftware.DeveloperAchievements.Website
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : Ninject.Web.NinjectHttpApplication
    {
        private NinjectControllerFactory _controllerFactory;

        internal static IKernel Kernel { get; private set; }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{action}/{key}", // URL with parameters
                new { controller = "Achievements", action = "LeaderBoard", key = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected override void OnApplicationStarted()
        {
            CodeGeneratorSettings.AddGlobalImport("ChadwickSoftware.DeveloperAchievements");
            CodeGeneratorSettings.AddGlobalImport("ChadwickSoftware.DeveloperAchievements.Website.Models");

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            ControllerBuilder.Current.SetControllerFactory(_controllerFactory);
        }

        protected override IKernel CreateKernel()
        {
            Kernel = new StandardKernel(new CoreBindingModule(), new DataAccessBindingModule());
            
            Kernel.Bind<HttpContext>().ToMethod(ctx => HttpContext.Current).InTransientScope();
            Kernel.Bind<HttpContextBase>().ToMethod(ctx => new HttpContextWrapper(HttpContext.Current)).InTransientScope();

            _controllerFactory = new NinjectControllerFactory(Kernel);

            return Kernel;
        }
    }
}