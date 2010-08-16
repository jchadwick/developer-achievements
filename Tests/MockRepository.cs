using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ChadwickSoftware.DeveloperAchievements
{
    public class MockRepository : IRepository
    {
        private readonly ICollection<object> _inMemoryRepository = new Collection<object>();

        protected IEnumerable<T> ObjectsOfType<T>()
        {
            return _inMemoryRepository.Where(x => x is T).Cast<T>();
        }

        public T Get<T>(long id) where T : IEntity
        {
            return ObjectsOfType<T>().Single(x => x.ID == id);
        }

        public T Get<T>(string key) where T : IEntity
        {
            return ObjectsOfType<T>().Single(x => x.Key == key);
        }

        public IQueryable<T> Query<T>() where T : IEntity
        {
            return ObjectsOfType<T>().AsQueryable();
        }

        public void Refresh<T>(T instance) where T : IEntity
        {
            
        }


        public void Save<T>(T instance) where T : IEntity
        {
            if (_inMemoryRepository.Contains(instance))
                return;
            else
                _inMemoryRepository.Add(instance);
        }

        public void SaveAll<T>(IEnumerable<T> instances) where T : IEntity
        {
            foreach (var instance in instances)
            {
                Save(instance);
            }
        }

        public void ExecuteSql(string sqlQuery)
        {
            
        }
    }
}