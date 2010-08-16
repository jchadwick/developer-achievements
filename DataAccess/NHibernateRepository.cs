using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace ChadwickSoftware.DeveloperAchievements.DataAccess
{
    public class NHibernateRepository : IRepository, IDisposable
    {
        public ISession Session
        {
            get { return _session; }
        }
        private readonly ISession _session;


        public NHibernateRepository(ISession session)
        {
            _session = session;
        }


        public virtual T Get<T>(long id) where T : IEntity
        {
            var result = Session.Get<T>(id);
            return ReferenceEquals(null, result) ? default(T) : result;
        }

        public virtual T Get<T>(string key) where T : IEntity
        {
            var criteria = Session.CreateCriteria(typeof(T));
            criteria.Add(Expression.Eq("Key", key));
            var result = criteria.UniqueResult<T>();
            return ReferenceEquals(null, result) ? default(T) : result;
        }

        public IQueryable<T> Query<T>() where T : IEntity
        {
            return Session.Linq<T>();
        }

        public void Refresh<T>(T instance) where T : IEntity
        {
            Session.Refresh(instance);
        }

        public void Delete<T>(T target) where T : IEntity
        {
            using (ITransaction tx = Session.BeginTransaction())
            {
                Session.Delete(target);
                tx.Commit();
            }
        }

        public void SaveAll<T>(IEnumerable<T> entities) where T : IEntity
        {
            using (var tx = Session.BeginTransaction())
            {
                foreach (var entity in entities)
                    InternalSave(entity);

                tx.Commit();
            }
        }

        public void ExecuteSql(string sqlQuery)
        {
            Session.CreateSQLQuery(sqlQuery).ExecuteUpdate();
        }

        public virtual void Save<T>(T entity) where T : IEntity
        {
            using (var tx = Session.BeginTransaction())
            {
                InternalSave(entity);
                tx.Commit();
            }
        }

        public void Save<T>(IEnumerable<T> targets) where T : IEntity
        {
            if (targets == null)
                throw new ArgumentNullException("targets");

            using (var tx = Session.BeginTransaction())
            {
                foreach (var entity in targets.Where(x => x != null))
                {
                    InternalSave(entity);
                }
                tx.Commit();
            }
        }

        private void InternalSave<T>(T entity) where T : IEntity
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Session.SaveOrUpdate(entity);
        }

        public void Flush()
        {
            Session.Flush();
        }

        public void Dispose()
        {
            Session.Dispose();
        }

        public static NHibernateRepository CreateForSingleUse()
        {
            var config = new MsSqlNHibernateConfiguration();
            var repository = new NHibernateRepository(config.CreateSessionFactory().OpenSession());
            return repository;
        }
    }
}
