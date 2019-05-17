using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Capstone.Data.Infrastructrure
{
    public interface IRepository<T> where T : class
    {
        // Marks an entity as new
        void Add(T entity);
        // Marks an entity as modified
        void Update(T entity);
        // Marks an entity to be removed
        void Delete(T enity);
        void Delete(Expression<Func<T, bool>> where);
        // Get an entity by int id
        T GetById(Guid id);
        // Gets all entities of type T
        IEnumerable<T> GetAll();
        // Gets entities using delegate
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
    }
}
