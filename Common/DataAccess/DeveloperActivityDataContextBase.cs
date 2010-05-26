using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Services;
using System.Linq;

namespace DeveloperAchievements.DataAccess
{
    public abstract class DeveloperActivityDataContextBase : IUpdatable, ICloneable, IExpandProvider, IDisposable
    {
        #region Implementation of IUpdatable

        public abstract object CreateResource(string containerName, string fullTypeName);
        public abstract object GetResource(IQueryable query, string fullTypeName);
        public abstract object ResetResource(object resource);
        public abstract void SetValue(object targetResource, string propertyName, object propertyValue);
        public abstract object GetValue(object targetResource, string propertyName);
        public abstract void SetReference(object targetResource, string propertyName, object propertyValue);
        public abstract void AddReferenceToCollection(object targetResource, string propertyName, object resourceToBeAdded);
        public abstract void RemoveReferenceFromCollection(object targetResource, string propertyName, object resourceToBeRemoved);
        public abstract void DeleteResource(object targetResource);
        public abstract void SaveChanges();
        public abstract object ResolveResource(object resource);
        public abstract void ClearChanges();

        #endregion

        #region Implementation of ICloneable

        public abstract object Clone();

        #endregion

        #region Implementation of IExpandProvider

        public abstract IEnumerable ApplyExpansions(IQueryable queryable, ICollection<ExpandSegmentCollection> expandPaths);

        #endregion

        #region Implementation of IDisposable

        public abstract void Dispose();

        #endregion

        public abstract IQueryable<T> Queryable<T>();
    }
}