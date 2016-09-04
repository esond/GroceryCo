using System;
using System.Collections.Generic;
using System.IO;
using GroceryCo.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GroceryCo.Repository
{
    public class LocalFileEntityRepository : IRepository
    {
        private const string RepositoryDirectoryName = @"GroceryCoData";

        private readonly string _directory;

        public LocalFileEntityRepository()
        {
        }

        public LocalFileEntityRepository(string directoryPath)
        {
            _directory = Path.Combine(directoryPath, RepositoryDirectoryName);

            Directory.CreateDirectory(_directory);
        }

        #region Implementation of IRepository

        public void Create<TEntity>(TEntity entity) where TEntity : Entity
        {
            if (entity == null)
                throw new ArgumentException("entity cannot be null", nameof(entity));

            using (StreamWriter file = File.AppendText(Path.Combine(_directory, typeof(TEntity).Name + ".json")))
            {
                file.WriteLine(JsonConvert.SerializeObject(entity));
            }
        }

        public IEnumerable<TEntity> Get<TEntity>() where TEntity : Entity
        {
            using (StreamReader file = File.OpenText(Path.Combine(_directory, typeof(TEntity).Name + ".json")))
            using (JsonTextReader reader = new JsonTextReader(file) {SupportMultipleContent = true, FloatParseHandling = FloatParseHandling.Decimal})
            {
                List<TEntity> entities = new List<TEntity>();

                while (reader.Read())
                {
                    JObject jEntity = (JObject)JToken.ReadFrom(reader);
                    TEntity entity = JsonConvert.DeserializeObject<TEntity>(jEntity.ToString());

                    entities.Add(entity);
                }

                return entities;
            }
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
