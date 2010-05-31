using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Ninject;
using Ninject.Modules;

namespace DeveloperAchievements.Util
{
    public class KernelFactory
    {
        [ImportMany(typeof(INinjectModule))]
        public IEnumerable<INinjectModule> Modules { get; set; }

        public IKernel GetKernel(string modulesDirectory)
        {
            if(Modules == null)
                MefifyMe(modulesDirectory);

            var kernel = new StandardKernel();

            foreach (var module in Modules)
            {
                Debug.WriteLine("Registering IoC module {0}...", new object[] { module.GetType().Name });
                kernel.Load(module);
            }

            return kernel;
        }

        private void MefifyMe(string modulesDirectory)
        {
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