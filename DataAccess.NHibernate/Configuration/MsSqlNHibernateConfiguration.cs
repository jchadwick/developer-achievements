using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using DeveloperAchievements.DataAccess.NHibernate.Mappings.Activities;
using FluentNHibernate.Cfg.Db;

namespace DeveloperAchievements.DataAccess.NHibernate.Configuration
{
    public class MsSqlNHibernateConfiguration : NHibernateConfiguration
    {
        public const string DefaultConnectionName = "DeveloperAchievements";
        private readonly string _connectionName;

        
        [CoverageExclude("Dude, this is just a constructor overload...")]
        public MsSqlNHibernateConfiguration() : this(DefaultConnectionName)
        {
        }

        public MsSqlNHibernateConfiguration(string connectionName)
        {
            _connectionName = connectionName ?? DefaultConnectionName;
        }


        public override void CreateDatabase()
        {
            InitializeDatabase();

            base.CreateDatabase();
        }

        [CoverageExclude("Some lines (for creating the database) are not necessarily run every test run")]
        private void InitializeDatabase()
        {
            var originalConnectionString = ConfigurationManager.ConnectionStrings[_connectionName].ConnectionString;
            var databaseName = Regex.Match(originalConnectionString, "[Initial Catalog|Database]=(?<database>[^;]*)").Groups["database"].Value;
            var connectionString = originalConnectionString.Replace(databaseName, "master");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var dbExists = (bool)new SqlCommand(string.Format("SELECT CAST(COUNT(name) AS BIT) FROM sys.databases WHERE name = N'{0}'", databaseName), connection).ExecuteScalar();
                if (dbExists)
                    return;

                new SqlCommand(string.Format("CREATE DATABASE {0};", databaseName), connection).ExecuteNonQuery();
            }

            using(SqlConnection connection = new SqlConnection(originalConnectionString))
            {
                connection.Open();
                new SqlCommand(string.Format("CREATE SCHEMA {0};", DeveloperActivityMapping.SchemaName), connection).ExecuteNonQuery();
            }
        }

        protected override IPersistenceConfigurer GetConnectionInfo()
        {
            return MsSqlConfiguration.MsSql2005
                .ConnectionString(c => c.FromConnectionStringWithKey(_connectionName))
                .ShowSql();
        }
    }
}