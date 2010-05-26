using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Web;
using Autofac;
using Autofac.Builder;
using Autofac.Integration.Web;

namespace DeveloperAchievements.Website
{
    public class ContainerProviderFactory
    {
        [ImportMany(typeof(IModule))]
        public IEnumerable<IModule> Modules { get; set; }

        public IContainerProvider GetContainerProvider()
        {
            if(Modules == null)
                MefifyMe();

            ContainerBuilder builder = new ContainerBuilder();

            foreach (var module in Modules)
            {
                Debug.WriteLine("Registering Autofac module {0}...", new object[] { module.GetType().Name });
                builder.RegisterModule(module);
            }

            return new ContainerProvider(builder.Build());
        }

        private void MefifyMe()
        {
            string modulesDirectory = HttpContext.Current.Server.MapPath("~/Modules");

            Debug.WriteLine("Loading Autofac modules from executing assembly and {0}...", new object[] { modulesDirectory });

            if(!Directory.Exists(modulesDirectory))
            {
                throw new ApplicationException("Modules directory " + modulesDirectory + " doesn't exist!");
            }

            ComposablePartCatalog catalog = new AggregateCatalog(
                    new AssemblyCatalog(Assembly.GetExecutingAssembly()),
                    new DirectoryCatalog(modulesDirectory)
                );
            CompositionContainer container = new CompositionContainer(catalog);

            container.ComposeParts(this);
        }
    }
}