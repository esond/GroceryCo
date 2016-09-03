using System;
using System.IO;
using GroceryCo.Model;

namespace GroceryCo.Repository
{
    public class LocalFileEntityRepository : IRepository
    {
        private const string RepositoryDirectoryName = @"GroceryCoData";

        public LocalFileEntityRepository()
        {
        }

        public LocalFileEntityRepository(string directoryPath)
        {
            Directory.CreateDirectory(Path.Combine(directoryPath, RepositoryDirectoryName));
        }

        #region Implementation of IRepository

        public void Create<TEntity>(TEntity entity) where TEntity : Entity
        {
            if (entity == null)
                throw new ArgumentException("entity cannot be null", nameof(entity));



            throw new NotImplementedException();
        }

        public TEntity Get<TEntity>(Guid id) where TEntity : Entity
        {
            throw new NotImplementedException();
        }

        public void Update<TEntity>(TEntity entity) where TEntity : Entity
        {
            throw new NotImplementedException();
        }

        public bool Delete<TEntity>(Guid id) where TEntity : Entity
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
