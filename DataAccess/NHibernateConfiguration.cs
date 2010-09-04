using System;
using System.Reflection;
using ChadwickSoftware.DeveloperAchievements.DataAccess.Mappings;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace ChadwickSoftware.DeveloperAchievements.DataAccess
{
    public abstract class NHibernateConfiguration : IDataConfiguration
    {
        protected virtual FluentConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                    _configuration = GetConfiguration();
                return _configuration;
            }
        }
        private FluentConfiguration _configuration;

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
                                    .AddFromAssemblyOf<AchievementMap>()
                                .Conventions
                                    .AddFromAssemblyOf<NHibernateConfiguration>());
        }

        protected abstract IPersistenceConfigurer GetConnectionInfo();
    }

    public class GlobalMappingConventions : IClassConvention
    {
        public void Apply(IClassInstance instance)
        {
            string className = instance.TableName.Replace("`", string.Empty);

            string tableName;

            if (className.EndsWith("y"))
                tableName = className.Substring(0, className.Length - 1) + "ies";
            else
                tableName = className + "s";

            instance.Table(tableName);
        }
    }

    public class CustomForeignKeyConvention : ForeignKeyConvention
    {
        protected override string GetKeyName(PropertyInfo property, Type type)
        {
            if (property == null)
                return type.Name + "ID";

            return property.Name + "ID";
        }
    }

    public class SqlKeywordsNameMappingPropertyConvention : IPropertyConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            if (instance.Name.Equals("Key", StringComparison.OrdinalIgnoreCase))
                instance.Column(String.Format("[{0}]", instance.Name));
        }
    }
}