using System;
using GroceryCo.Model;

namespace GroceryCo.Repository
{
    public interface IRepository
    {
        void Create<TEntity>(TEntity entity) where TEntity : Entity;

        TEntity Get<TEntity>(Guid id) where TEntity : Entity;

        void Update<TEntity>(TEntity entity) where TEntity : Entity;

        bool Delete<TEntity>(Guid id) where TEntity : Entity;
    }
}
