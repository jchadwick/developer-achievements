using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using FluentNHibernate.Cfg.Db;

namespace ChadwickSoftware.DeveloperAchievements.DataAccess
{
    public class MsSqlNHibernateConfiguration : NHibernateConfiguration
    {
        public const string DefaultConnectionName = "DeveloperAchievements";
        private const string DefaultSchemaName = "DeveloperAchievements";

        private readonly string _connectionName;

        protected string SchemaName
        {
            get { return _schemaName ?? DefaultSchemaName; }
            set { _schemaName = value; }
        }
        private string _schemaName;

        protected SqlDatabase Database
        {
            get { return _database ?? (_database = SqlDatabase.BuildFromConnection(_connectionName)); }
            set { _database = value; }
        }
        private SqlDatabase _database;

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

        public override void DropDatabase()
        {
            SqlConnection.ClearAllPools();

            SqlDatabase database = Database;
            using(SqlConnection connection = new SqlConnection(database.MasterDatabaseConnectionString))
            {
                connection.Open();
                
                new SqlCommand(string.Format("ALTER DATABASE {0} SET SINGLE_USER WITH ROLLBACK IMMEDIATE", database.DatabaseName), connection).ExecuteNonQuery();
                new SqlCommand(string.Format("DROP DATABASE {0};", database.DatabaseName), connection).ExecuteNonQuery();
            }
        }

        [CoverageExclude("Some lines (for creating the database) are not necessarily run every test run")]
        private void InitializeDatabase()
        {
            SqlDatabase database = Database;

            using (SqlConnection connection = new SqlConnection(database.MasterDatabaseConnectionString))
            {
                connection.Open();

                var dbExists = (bool)new SqlCommand(string.Format("SELECT CAST(COUNT(name) AS BIT) FROM sys.databases WHERE name = N'{0}'", database.DatabaseName), connection).ExecuteScalar();
                if (dbExists)
                    return;

                new SqlCommand(string.Format("CREATE DATABASE {0};", database.DatabaseName), connection).ExecuteNonQuery();
            }

            using (SqlConnection connection = new SqlConnection(database.ConnectionString))
            {
                connection.Open();
                new SqlCommand(string.Format("CREATE SCHEMA {0};", SchemaName), connection).ExecuteNonQuery();
            }
        }

        protected override IPersistenceConfigurer GetConnectionInfo()
        {
            return MsSqlConfiguration.MsSql2005
                .ConnectionString(c => c.FromConnectionStringWithKey(_connectionName))
                .ShowSql();
        }


        public class SqlDatabase
        {
            public string ConnectionName { get; set; }
            public string ConnectionString { get; set; }
            public string DatabaseName { get; set; }
            public string MasterDatabaseConnectionString { get; set; }

            public static SqlDatabase BuildFromConnection(string connectionName)
            {
                var connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
                var databaseName = Regex.Match(connectionString, "[Initial Catalog|Database]=(?<database>[^;]*)").Groups["database"].Value;
                var masterDatabaseConnectionString = connectionString.Replace(databaseName, "master");

                SqlDatabase database = new SqlDatabase
                                           {
                                               ConnectionName = connectionName,
                                               ConnectionString = connectionString,
                                               DatabaseName = databaseName,
                                               MasterDatabaseConnectionString = masterDatabaseConnectionString
                                           };
                return database;
            }

        }
    }
}