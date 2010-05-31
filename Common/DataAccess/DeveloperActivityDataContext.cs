using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Services;
using System.Linq;
using DeveloperAchievements.Achievements;
using DeveloperAchievements.Activities;

namespace DeveloperAchievements.DataAccess
{
    public class DeveloperActivityDataContext : IUpdatable, ICloneable, IExpandProvider, IDisposable
    {
        private readonly DeveloperActivityDataContextBase _baseContext;

        public DeveloperActivityDataContext(DeveloperActivityDataContextBase baseContext)
        {
            _baseContext = baseContext;
        }


        public IQueryable<AchievementDescriptor> Achievements
        {
            get { return _baseContext.Queryable<AchievementDescriptor>(); }
        }

        public IQueryable<Achievement> Awards
        {
            get { return _baseContext.Queryable<Achievement>(); }
        }

        public IQueryable<User> Developers
        {
            get { return _baseContext.Queryable<User>(); }
        }

        public IQueryable<Build> Builds
        {
            get { return _baseContext.Queryable<Build>(); }
        }

        public IQueryable<BuildResult> BuildResults
        {
            get
            {

                return new List<BuildResult>
                           {
                               BuildResult.Fixed,
                               BuildResult.Success,
                               BuildResult.Failed
                           }.AsQueryable();
            }
        }

        public IQueryable<CheckIn> CheckIns
        {
            get { return _baseContext.Queryable<CheckIn>(); }
        }

        public IQueryable<DeveloperHistory> HistoryEntries
        {
            get { return _baseContext.Queryable<DeveloperHistory>(); }
        }


        #region Base Context proxy methods

        public object CreateResource(string containerName, string fullTypeName)
        {
            return _baseContext.CreateResource(containerName, fullTypeName);
        }

        public object GetResource(IQueryable query, string fullTypeName)
        {
            return _baseContext.GetResource(query, fullTypeName);
        }

        public object ResetResource(object resource)
        {
            return _baseContext.ResetResource(resource);
        }

        public void SetValue(object targetResource, string propertyName, object propertyValue)
        {
            _baseContext.SetValue(targetResource, propertyName, propertyValue);
        }

        public object GetValue(object targetResource, string propertyName)
        {
            return _baseContext.GetValue(targetResource, propertyName);
        }

        public void SetReference(object targetResource, string propertyName, object propertyValue)
        {
            _baseContext.SetReference(targetResource, propertyName, propertyValue);
        }

        public void AddReferenceToCollection(object targetResource, string propertyName, object resourceToBeAdded)
        {
            _baseContext.AddReferenceToCollection(targetResource, propertyName, resourceToBeAdded);
        }

        public void RemoveReferenceFromCollection(object targetResource, string propertyName, object resourceToBeRemoved)
        {
            _baseContext.RemoveReferenceFromCollection(targetResource, propertyName, resourceToBeRemoved);
        }

        public void DeleteResource(object targetResource)
        {
            _baseContext.DeleteResource(targetResource);
        }

        public void SaveChanges()
        {
            _baseContext.SaveChanges();
        }

        public object ResolveResource(object resource)
        {
            return _baseContext.ResolveResource(resource);
        }

        public void ClearChanges()
        {
            _baseContext.ClearChanges();
        }

        public object Clone()
        {
            return _baseContext.Clone();
        }

        public IEnumerable ApplyExpansions(IQueryable queryable, ICollection<ExpandSegmentCollection> expandPaths)
        {
            return _baseContext.ApplyExpansions(queryable, expandPaths);
        }

        public void Dispose()
        {
            _baseContext.Dispose();
        }
        #endregion

    }
}