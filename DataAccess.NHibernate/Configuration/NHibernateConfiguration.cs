using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace DeveloperAchievements.DataAccess.NHibernate.Configuration
{
    public abstract class NHibernateConfiguration : IDataConfiguration
    {
        private FluentConfiguration _configuration;
        protected virtual FluentConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                    _configuration = GetConfiguration();
                return _configuration;
            }
        }

        public virtual void CreateDatabase()
        {
            new SchemaExport(Configuration.BuildConfiguration()).Create(true, true);
        }

        public virtual ISessionFactory CreateSessionFactory()
        {
            return Configuration.BuildSessionFactory();
        }

        protected FluentConfiguration GetConfiguration()
        {
            return Fluently.Configure()
                .Database(GetConnectionInfo())
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()));
        }

        protected abstract IPersistenceConfigurer GetConnectionInfo();
    }
}