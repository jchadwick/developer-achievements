using System;
using System.Web;
using DeveloperAchievements.Util;
using Ninject;
using Ninject.Web;

namespace DeveloperAchievements.Website
{
    public class Global : NinjectHttpApplication
    {
        private static IKernel _kernel;

        internal static IKernel Container
        {
            get { return _kernel; }
        }

        protected override IKernel CreateKernel()
        {
            if(_kernel == null)
                _kernel = new KernelFactory().GetKernel(HttpContext.Current.Server.MapPath("~/Modules"));

            return _kernel;
        }

        protected void Application_Start(object sender, EventArgs e)
        {
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
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