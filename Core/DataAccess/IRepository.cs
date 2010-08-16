using System.Collections.Generic;
using System.Linq;

namespace ChadwickSoftware.DeveloperAchievements
{
    public interface IRepository
    {
        T Get<T>(long id) where T : IEntity;
        
        T Get<T>(string key) where T : IEntity;

        IQueryable<T> Query<T>() where T : IEntity;

        void Refresh<T>(T instance) where T : IEntity;

        void Save<T>(T instance) where T : IEntity;

        void SaveAll<T>(IEnumerable<T> instances) where T : IEntity;
        
        void ExecuteSql(string sqlQuery);
    }
}