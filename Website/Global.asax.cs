﻿using System;
using System.IO;
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

        public MvcApplication()
        {
            BeginRequest += CreateDatabaseOnFirstRun;
        }

        private void CreateDatabaseOnFirstRun(object sender, EventArgs e)
        {
            string triggerFilePath = Server.MapPath("~/firstrun");

            if (File.Exists(triggerFilePath))
            {
                NHibernateConfiguration configuration = Kernel.Get<NHibernateConfiguration>();
                configuration.CreateDatabase();

                try
                {
                    File.Delete(triggerFilePath);
                }
                catch(Exception ex)
                {
                    string message =
                        string.Format(
                            "Your first-run configuration succeeded, but we were unable to remove the 'firstrun' trigger file.  Please delete this file ({0}) manually, otherwise you're going to rebuild your database every time!",
                            triggerFilePath);
                    
                    throw new ApplicationException(message, ex);
                }
            }
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
            NinjectSettings settings = new NinjectSettings() { InjectNonPublic = true };

            Kernel = new StandardKernel(settings, new CoreBindingModule(), new DataAccessBindingModule());
            
            Kernel.Bind<HttpContext>().ToMethod(ctx => HttpContext.Current).InTransientScope();
            Kernel.Bind<HttpContextBase>().ToMethod(ctx => new HttpContextWrapper(HttpContext.Current)).InTransientScope();

            _controllerFactory = new NinjectControllerFactory(Kernel);

            return Kernel;
        }
    }
}