using System;
using System.IO;
using GroceryCo.Model;
using GroceryCo.Repository;
using NUnit.Framework;

namespace GroceryCo.UnitTests.Repository
{
    [TestFixture]
    public class LocalFileEntityRepositoryTester
    {
        private IRepository _repository = new LocalFileEntityRepository();

        [SetUp]
        public void SetUp()
        {
            _repository = new LocalFileEntityRepository();
        }

        [Test]
        public void attempting_to_create_repository_in_inaccessible_folder_throws_DirectoryNotFoundException()
        {
            Assert.Throws<DirectoryNotFoundException>(
                () => _repository = new LocalFileEntityRepository("Z:/PlanetMars"));
        }

        [Test]
        public void attempting_to_create_null_throws_argument_exception()
        {
            _repository = new LocalFileEntityRepository();

            Assert.Throws<ArgumentException>(() => _repository.Create((GroceryItem)null));
        }
    }
}
