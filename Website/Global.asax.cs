using System;
using Autofac.Integration.Web;

namespace DeveloperAchievements.Website
{
    public class Global : System.Web.HttpApplication
    {
        internal static IContainerProvider Container { get; private set; }

        private static void InitializeInversionOfControlContainer()
        {
            ContainerProviderFactory containerProviderFactory = new ContainerProviderFactory();
            Container = containerProviderFactory.GetContainerProvider();
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            InitializeInversionOfControlContainer();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            Container.DisposeRequestContainer();
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}