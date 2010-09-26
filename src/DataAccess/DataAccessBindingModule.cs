using NHibernate;
using Ninject;
using Ninject.Modules;

namespace ChadwickSoftware.DeveloperAchievements.DataAccess
{
    public class DataAccessBindingModule : NinjectModule
    {
        public override void Load()
        {
            Bind<NHibernateConfiguration>()
                .To<MsSqlNHibernateConfiguration>()
                .InSingletonScope();

            Bind<ISessionFactory>()
                .ToMethod(x => x.Kernel.Get<NHibernateConfiguration>().CreateSessionFactory())
                .InSingletonScope();

            Bind<ISession>()
                .ToMethod(x => x.Kernel.Get<ISessionFactory>().OpenSession())
                .InRequestScope();

            Bind<IRepository>()
                .To<NHibernateRepository>()
                .InRequestScope();
        }
    }
}
