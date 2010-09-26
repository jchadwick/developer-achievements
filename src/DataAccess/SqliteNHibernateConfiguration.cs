using System.IO;
using FluentNHibernate.Cfg.Db;

namespace ChadwickSoftware.DeveloperAchievements.DataAccess
{
    public class SqliteNHibernateConfiguration : NHibernateConfiguration
    {
        private readonly string _databaseFilename;

        public SqliteNHibernateConfiguration(string databaseFilename)
        {
            _databaseFilename = databaseFilename;
        }

        public override void DropDatabase()
        {
            if(File.Exists(_databaseFilename))
                File.Delete(_databaseFilename);
        }

        protected override IPersistenceConfigurer GetConnectionInfo()
        {
            return SQLiteConfiguration.Standard.UsingFile(_databaseFilename);
        }
    }
}