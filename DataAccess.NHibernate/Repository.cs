using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Linq;
using Expression=NHibernate.Criterion.Expression;

namespace DeveloperAchievements.DataAccess.NHibernate
{
    public class Repository : IRepository, IDisposable
    {
        private readonly ISession _session;
        public ISession Session
        {
            get { return _session; }
        }


        public Repository(ISession Session)
        {
            _session = Session;
        }


        public virtual T FindByKey<T>(string key) where T : KeyedEntity
        {
            var criteria = Session.CreateCriteria(typeof(T));
            criteria.Add(Expression.Eq("Key", key));
            var result = criteria.UniqueResult<T>();
            return ReferenceEquals(null, result) ? null : result;
        }

        public IEnumerable<T> FindByQuery<T>(Func<T, bool> query) where T : KeyedEntity
        {
            return Session.Linq<T>().Where(query).ToArray();
        }

        public void SaveAll<T>(IEnumerable<T> entities) where T : KeyedEntity
        {
            using (var tx = Session.BeginTransaction())
            {
                foreach (var entity in entities)
                    InternalSave(entity);

                tx.Commit();
            }
        }

        public void Delete<T>(T target) where T : KeyedEntity
        {
            using (ITransaction tx = Session.BeginTransaction())
            {
                Session.Delete(target);
                tx.Commit();
            }
        }

        public T Find<T>(long id) where T : KeyedEntity
        {
            var result = Session.Get<T>(id);
            return ReferenceEquals(null, result) ? null : result;
        }

        public T FindBy<T>(Expression<Func<T, bool>> where) where T : KeyedEntity
        {
            return Session.Linq<T>().Where(where).FirstOrDefault();
        }

        public IEnumerable<T> Query<T>(Expression<Func<T, bool>> where) where T : KeyedEntity
        {
            return Session.Linq<T>().Where(where);
        }

        public virtual void Save<T>(T entity) where T : KeyedEntity
        {
            using (var tx = Session.BeginTransaction())
            {
                InternalSave(entity);
                tx.Commit();
            }
        }

        private void InternalSave<T>(T entity) where T : KeyedEntity
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            //entity.ValidateWithException();
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
    }
}