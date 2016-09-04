using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GroceryCo.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GroceryCo.Repository
{
    public class LocalFileEntityRepository : IRepository
    {
        private readonly string _directoryPath;

        public LocalFileEntityRepository()
        {
        }

        public LocalFileEntityRepository(string directoryPath)
        {
            _directoryPath = directoryPath;
            Directory.CreateDirectory(_directoryPath);
        }

        #region Implementation of IRepository

        public void Create<TEntity>(TEntity entity) where TEntity : Entity
        {
            if (entity == null)
                throw new ArgumentException("entity cannot be null", nameof(entity));

            if (EntityExists<TEntity>(entity.Id))
                throw new InvalidOperationException(
                    $"There is already a {typeof(TEntity).Name} with Id {entity.Id} in the repository.");

            using (StreamWriter file = File.AppendText(GetFilePathForType<TEntity>()))
            {
                file.WriteLine(JsonConvert.SerializeObject(entity, Formatting.Indented));
            }
        }

        public IEnumerable<TEntity> GetAll<TEntity>() where TEntity : Entity
        {
            if (!File.Exists(Path.Combine(_directoryPath, typeof(TEntity).Name + ".json")))
                return Enumerable.Empty<TEntity>();

            using (StreamReader file = File.OpenText(GetFilePathForType<TEntity>()))
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

        public void Update<TEntity>(TEntity toUpdate) where TEntity : Entity
        {
            if (!EntityExists<TEntity>(toUpdate.Id))
                throw new InvalidOperationException(
                    $"A(n) {typeof(TEntity).Name} with Id {toUpdate.Id} does not exist in the repository.");

            TEntity updated = Copy(toUpdate);

            Delete(toUpdate);

            Create(updated);
        }

        public bool Delete<TEntity>(TEntity toRemove) where TEntity : Entity
        {
            if (toRemove == null)
                throw new ArgumentException("toRemove cannot be null", nameof(toRemove));

            ICollection<TEntity> entities = GetAll<TEntity>().ToList();

            entities.Remove(entities.Single(e => e.Id == toRemove.Id));

            File.WriteAllText(GetFilePathForType<TEntity>(), string.Empty);

            foreach (TEntity entity in entities)
            {
                Create(entity);
            }

            return true;
        }

        #endregion

        #region Helpers

        private static TEntity Copy<TEntity>(TEntity source) where TEntity : Entity
        {
            string serialized = JsonConvert.SerializeObject(source);

            return JsonConvert.DeserializeObject<TEntity>(serialized);
        }

        private string GetFilePathForType<TEntity>() where TEntity : Entity
        {
            return Path.Combine(_directoryPath, typeof(TEntity).Name + ".json");
        }

        private bool EntityExists<TEntity>(Guid id) where TEntity : Entity
        {
            IEnumerable<TEntity> entities = GetAll<TEntity>();

            return entities.SingleOrDefault(e => e.Id == id) != null;
        }

        #endregion
    }
}
