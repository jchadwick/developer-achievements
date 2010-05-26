using System.ComponentModel.Composition;
using Autofac;
using Autofac.Builder;
using DeveloperAchievements.Activities;
using DeveloperAchievements.DataAccess.NHibernate.Configuration;
using NHibernate;

namespace DeveloperAchievements.DataAccess.NHibernate
{
    [Export(typeof(IModule))]
    public class DependencyModule : Module 
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<MsSqlNHibernateConfiguration>().As<NHibernateConfiguration>().SingletonScoped();
            
            builder.Register(x => x.Resolve<NHibernateConfiguration>()).As<IDataConfiguration>();

            builder.Register<DeveloperActivityRepository>().As<IDeveloperActivityRepository>();

            builder
                .Register<NHibernateDeveloperActivityDataContext>()
                .As<DeveloperActivityDataContextBase>()
                .ContainerScoped();
            
            builder
                .Register<DeveloperActivityDataContext>()
                .ContainerScoped();
            
            builder
                .Register(x => x.Resolve<NHibernateConfiguration>().CreateSessionFactory())
                .As<ISessionFactory>()
                .SingletonScoped();
            builder
                .Register(x => x.Resolve<ISessionFactory>().OpenSession())
                .As<ISession>()
                .ContainerScoped();
            builder
                .Register<Repository>()
                .As<IRepository>()
                .ContainerScoped();
        }
    }
}
