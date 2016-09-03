using System;
using System.Collections.Generic;
using GroceryCo.Model;

namespace GroceryCo.Repository
{
    public interface IRepository
    {
        void Create<TEntity>(TEntity entity) where TEntity : Entity;

        IEnumerable<TEntity> Get<TEntity>() where TEntity : Entity;

        void Update<TEntity>(Guid id) where TEntity : Entity;

        bool Delete<TEntity>(Guid id) where TEntity : Entity;
    }
}
