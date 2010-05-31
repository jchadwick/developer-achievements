using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DeveloperAchievements
{
    public interface IRepository
    {
        void Delete<T>(T target) where T : KeyedEntity;

        T Find<T>(long id) where T : KeyedEntity;

        T FindByKey<T>(string key) where T : KeyedEntity;

        T FindBy<T>(Expression<Func<T, bool>> where) where T : KeyedEntity;

        IEnumerable<T> Query<T>(Expression<Func<T, bool>> where) where T : KeyedEntity;

        void Save<T>(T target) where T : KeyedEntity;

        void Save<T>(IEnumerable<T> targets) where T : KeyedEntity;
    }
}