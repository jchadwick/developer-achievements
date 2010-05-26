using System.Collections;
using System.Collections.Generic;
using System.Data.Services;
using System.Linq;
using System.Reflection;
using NHibernate;
using NHibernate.Linq;

namespace DeveloperAchievements.DataAccess.NHibernate
{
    public class NHibernateDeveloperActivityDataContext : DeveloperActivityDataContextBase
    {
        private readonly NHibernateContextImpl _baseContext;

        protected ISession Session
        {
            get { return _baseContext.Session; }
        }

        public NHibernateDeveloperActivityDataContext(ISession session)
        {
            _baseContext = new NHibernateContextImpl(session);
        }


        public override object CreateResource(string containerName, string fullTypeName)
        {
            return _baseContext.ExecuteMethod("CreateResource", containerName, fullTypeName);
        }

        public override object GetResource(IQueryable query, string fullTypeName)
        {
            return _baseContext.ExecuteMethod("GetResource", query, fullTypeName);
        }

        public override object ResetResource(object resource)
        {
            return _baseContext.ExecuteMethod("ResetResource", resource);
        }

        public override void SetValue(object targetResource, string propertyName, object propertyValue)
        {
            _baseContext.ExecuteMethod("SetValue", targetResource, propertyName, propertyValue);
        }

        public override object GetValue(object targetResource, string propertyName)
        {
            return _baseContext.ExecuteMethod("GetValue", targetResource, propertyName);
        }

        public override void SetReference(object targetResource, string propertyName, object propertyValue)
        {
            _baseContext.ExecuteMethod("SetReference", targetResource, propertyName, propertyValue);
        }

        public override void AddReferenceToCollection(object targetResource, string propertyName, object resourceToBeAdded)
        {
            _baseContext.ExecuteMethod("AddReferenceToCollection", targetResource, propertyName, resourceToBeAdded);
        }

        public override void RemoveReferenceFromCollection(object targetResource, string propertyName, object resourceToBeRemoved)
        {
            _baseContext.ExecuteMethod("RemoveReferenceFromCollection", targetResource, propertyName, resourceToBeRemoved);
        }

        public override void DeleteResource(object targetResource)
        {
            _baseContext.ExecuteMethod("DeleteResource", targetResource);
        }

        public override void SaveChanges()
        {
            _baseContext.ExecuteMethod("SaveChanges");
        }

        public override object ResolveResource(object resource)
        {
            return _baseContext.ExecuteMethod("ResolveResource", resource);
        }

        public override void ClearChanges()
        {
            _baseContext.ExecuteMethod("ClearChanges");
        }

        public override object Clone()
        {
            return _baseContext.ExecuteMethod("Clone");
        }

        public override IEnumerable ApplyExpansions(IQueryable queryable, ICollection<ExpandSegmentCollection> expandPaths)
        {
            return (IEnumerable)_baseContext.ExecuteMethod("ApplyExpansions", queryable, expandPaths);
        }

        public override void Dispose()
        {
            _baseContext.Dispose();
        }

        public override IQueryable<T> Queryable<T>()
        {
            return Session.Linq<T>();
        }


        protected class NHibernateContextImpl : NHibernateContext
        {
            protected IDictionary<string, MethodInfo> Methods
            {
                get
                {
                    if (_methods == null)
                    {
                        lock (MethodsLock)
                        {
                            if (_methods == null)
                            {
                                var methods =
                                    from method in GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                                    select new { method.Name, method };
                                
                                _methods = methods.ToDictionary(x => x.Name, y => y.method);
                            }
                        }
                    }
                    return _methods;
                }
            }
            private static IDictionary<string, MethodInfo> _methods;
            private static readonly object MethodsLock = new object();

            public NHibernateContextImpl(ISession session)
                : base(session)
            {
            }

            public object ExecuteMethod(string methodName, params object[] parameters)
            {
                return _methods[methodName].Invoke(this, parameters);
            }
        }
    }
}
