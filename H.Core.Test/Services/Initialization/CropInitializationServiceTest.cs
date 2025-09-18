using System;
using H.Core.Models;
using H.Core.Providers.Energy;
using H.Core.Services.Initialization;
using H.Infrastructure.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace H.Core.Test.Services.Initialization
{
    [TestClass]
    public class CropInitializationServiceTest
    {
        #region Fields

        private Mock<ICacheService> _cacheServiceMock;
        private Mock<ITable50FuelEnergyEstimatesProvider> _table50ProviderMock;
        private CropInitializationService _service;

        #endregion

        #region Initialialization

        [TestInitialize]
        public void Setup()
        {
            _cacheServiceMock = new Mock<ICacheService>();
            _table50ProviderMock = new Mock<ITable50FuelEnergyEstimatesProvider>();
            _service = new CropInitializationService(_cacheServiceMock.Object, _table50ProviderMock.Object);
        }

        #endregion

        #region Public Methods
        
        [TestMethod]
        public void Constructor_WithNullCacheService_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
                new CropInitializationService(null, _table50ProviderMock.Object));
        }

        [TestMethod]
        public void Constructor_WithNullTable50Provider_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
                new CropInitializationService(_cacheServiceMock.Object, null));
        }

        [TestMethod]
        public void Initialize_CallsInitializeFuelEnergy()
        {
            // Arrange
            var farm = new Farm();
            var serviceMock = new Mock<CropInitializationService>(_cacheServiceMock.Object, _table50ProviderMock.Object) { CallBase = true };
            serviceMock.Setup(s => s.InitializeFuelEnergy(farm)).Verifiable();

            // Act
            serviceMock.Object.Initialize(farm);

            // Assert
            serviceMock.Verify(s => s.InitializeFuelEnergy(farm), Times.Once);
        } 

        #endregion
    }
}