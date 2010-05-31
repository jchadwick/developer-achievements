using DeveloperAchievements.Activities;
using DeveloperAchievements.DataAccess.NHibernate.Mappings;
using DeveloperAchievements.DataAccess.NHibernate.Mappings.Activities;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
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

        public abstract void DropDatabase();

        public virtual ISessionFactory CreateSessionFactory()
        {
            return Configuration.BuildSessionFactory();
        }

        protected FluentConfiguration GetConfiguration()
        {
            return Fluently.Configure()
                .Database(GetConnectionInfo())
                .Mappings(m => m.FluentMappings
                                .AddFromAssemblyOf<DeveloperMapping>()
                                .Conventions.Add(
                                    PrimaryKey.Name.Is(x => "ID"),
                                    ForeignKey.EndsWith("ID")
                                )
                        );
        }

        protected abstract IPersistenceConfigurer GetConnectionInfo();
    }
}