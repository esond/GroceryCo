using System;
using System.IO;
using System.Linq;
using GroceryCo.Exceptions;
using GroceryCo.Model;
using GroceryCo.Repository;
using Newtonsoft.Json;
using NUnit.Framework;

namespace GroceryCo.IntegrationTests.Repository
{
    [TestFixture]
    public class LocalFileEntityRepositoryTester
    {
        private IRepository _repository;

        private string _repositoryFolderName;

        [SetUp]
        public void SetUp()
        {
            _repositoryFolderName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "GroceryCo.IntegrationTests");

            _repository = new LocalFileEntityRepository(_repositoryFolderName);
        }

        [TearDown]
        public void TearDown()
        {
            Directory.Delete(_repositoryFolderName, true);
        }

        [Test]
        public void creating_an_entity_and_retrieving_it_yields_entity_with_equal_values()
        {
            GroceryItem newItem = new GroceryItem("Apple", 2.50m);
            _repository.Create(newItem);

            GroceryItem retrievedItem = _repository.GetAll<GroceryItem>().Single(gi => gi.Id == newItem.Id);

            string expected = JsonConvert.SerializeObject(newItem);
            string actual = JsonConvert.SerializeObject(retrievedItem);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void updating_a_nonexistent_entity_throws_EntityNotFoundException()
        {
            Assert.Throws<EntityNotFoundException>(() => _repository.Update(new GroceryItem("foo", decimal.Zero)));
        }

        [Test]
        public void deleted_entities_cannot_be_found()
        {
            GroceryItem newItem = new GroceryItem("Apple", 2.50m);
            _repository.Create(newItem);
            _repository.Delete<GroceryItem>(newItem.Id);

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<InvalidOperationException>(() => _repository.GetAll<GroceryItem>().Single(gi => gi.Id == newItem.Id));
        }
    }
}
