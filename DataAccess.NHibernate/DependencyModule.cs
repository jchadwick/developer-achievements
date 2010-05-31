using System.ComponentModel.Composition;
using DeveloperAchievements.Activities;
using DeveloperAchievements.DataAccess.NHibernate.Configuration;
using NHibernate;
using Ninject;
using Ninject.Modules;

namespace DeveloperAchievements.DataAccess.NHibernate
{
    [Export(typeof(INinjectModule))]
    public class DependencyModule : NinjectModule
    {
        public override void Load()
        {

            Bind<NHibernateConfiguration>()
                .To<MsSqlNHibernateConfiguration>()
                .InSingletonScope();

            Bind<IDataConfiguration>()
                .To<MsSqlNHibernateConfiguration>()
                .InSingletonScope();

            Bind<ISessionFactory>()
                .ToMethod(x => x.Kernel.Get<NHibernateConfiguration>().CreateSessionFactory())
                .InSingletonScope();


            Bind<DeveloperActivityDataContextBase>()
                .To<NHibernateDeveloperActivityDataContext>()
                .InRequestScope();

            Bind<DeveloperActivityDataContextBase>()
                .To<NHibernateDeveloperActivityDataContext>()
                .InRequestScope();

            Bind<ISession>()
                .ToMethod(x => x.Kernel.Get<ISessionFactory>().OpenSession())
                .InRequestScope();

            Bind<IRepository>()
                .To<Repository>()
                .InRequestScope();

        }
    }
}
