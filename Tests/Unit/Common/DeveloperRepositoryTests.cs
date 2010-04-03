using System;
using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace DeveloperAchievements.Unit.Common
{
    [TestFixture]
    public class DeveloperRepositoryTests
    {
        private Mock<IRepository> _baseRepository;


        [SetUp]
        public void SetUp()
        {
            _baseRepository = new Mock<IRepository>();
        }
        

        [Test]
        public void ShouldCreateNewDeveloperWhenOneDoesNotExist()
        {
            _baseRepository
                .Setup(x => x.FindByKey<Developer>("testuser"))
                .Returns((Developer)null);
            _baseRepository
                .Setup(x => x.Save(It.IsAny<Developer>()))
                .Verifiable();

            var developerRepository = new DeveloperRepository(_baseRepository.Object);
            developerRepository.GetOrCreate("testuser");

            _baseRepository.VerifyAll();
        }

        [Test]
        public void ShouldReturnExistingDeveloperForAGivenUsername()
        {
            var expectedDeveloper = new Developer();

            _baseRepository
                .Setup(x => x.FindByKey<Developer>("testuser"))
                .Returns(expectedDeveloper);

            var developerRepository = new DeveloperRepository(_baseRepository.Object);
            var actualDeveloper = developerRepository.GetOrCreate("testuser");

            Assert.That(actualDeveloper, Is.SameAs(expectedDeveloper));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldNotAllowRetrievalOfDevelopersWithANullOrEmptyUsername()
        {
            var developerRepository = new DeveloperRepository(null);
            developerRepository.GetOrCreate(null);
        }
    }
}