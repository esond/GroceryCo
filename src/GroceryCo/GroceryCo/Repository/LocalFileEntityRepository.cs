using System;
using System.Collections.Generic;
using System.IO;
using GroceryCo.Model;

namespace GroceryCo.Repository
{
    public class LocalFileEntityRepository : IRepository
    {
        public LocalFileEntityRepository()
        {
        }

        public LocalFileEntityRepository(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
        }

        public void Create<TEntity>(TEntity entity) where TEntity : Entity
        {
            if (entity == null)
                throw new ArgumentException("entity cannot be null", nameof(entity));

            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> Get<TEntity>() where TEntity : Entity
        {
            throw new NotImplementedException();
        }

        public void Update<TEntity>(Guid id) where TEntity : Entity
        {
            throw new NotImplementedException();
        }

        public bool Delete<TEntity>(Guid id) where TEntity : Entity
        {
            throw new NotImplementedException();
        }
    }
}
