using System.Linq;
using System.Reflection;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Linq;

namespace Stupid
{
    public class AwesomeDataContext : NHibernateContext
    {
        public AwesomeDataContext()
        {
        }

        public AwesomeDataContext(ISession session) : base(session)
        {
            
        }

        protected override ISession ProvideSession()
        {
            if (_session == null)
            {
                FluentConfiguration configuration = 
                Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2005.ConnectionString(c => c.FromConnectionStringWithKey("DeveloperAchievements")).ShowSql())
                    .Mappings(m => m.FluentMappings.Add<DeveloperMapper>());

                var factory = configuration.BuildSessionFactory();
                _session = factory.OpenSession();
            }

            return _session;
        }
        private ISession _session;


        public IQueryable<Developer> Developers
        {
            get
            {
                var developers = Session.Linq<Developer>();
                return developers;
            }
        }
    }
}