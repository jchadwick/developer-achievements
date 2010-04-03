using DeveloperAchievements.DataAccess.NHibernate;
using DeveloperAchievements.DataAccess.NHibernate.Configuration;
using NHibernate;
using NUnit.Framework;

namespace DeveloperAchievements.Integration.DataAccess.NHibernate
{
    public class RepositoryTestBase
    {
        internal const string DatabaseName = "DeveloperAchievements_Integration";

        private static readonly object DatabaseInitializationLock = new object();
        private static bool _databaseInitialized;

        private static ISessionFactory _sessionFactory;
        protected static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                    _sessionFactory = new MsSqlNHibernateConfiguration(DatabaseName).CreateSessionFactory();
                return _sessionFactory;
            }
        }

        private Repository _repository;
        protected Repository Repository
        {
            get
            {
                if (_repository == null)
                    _repository = new Repository(SessionFactory.OpenSession());
                return _repository;
            }
            set { _repository = value; }
        }


        [TestFixtureSetUp]
        public virtual void TestFixtureSetUp()
        {
            InitializeDatabase(true);
        }

        protected static void InitializeDatabase(bool abortIfInitialized)
        {
            lock (DatabaseInitializationLock)
            {
                if (_databaseInitialized && abortIfInitialized)
                    return;

                _databaseInitialized = false;

                var configuration = new MsSqlNHibernateConfiguration(DatabaseName);
                configuration.CreateDatabase();

                _databaseInitialized = true;
            }
        }
    }
}