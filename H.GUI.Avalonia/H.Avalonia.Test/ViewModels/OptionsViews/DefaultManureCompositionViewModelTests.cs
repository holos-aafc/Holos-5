using H.Core.Models;
using H.Core.Providers.Animals;
using H.Core.Services.StorageService;
using Moq;
using Prism.Events;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews.Tests
{
    [TestClass]
    public class DefaultManureCompositionViewModelTests
    {
        private DefaultManureCompositionViewModel _viewModel;
        private Mock<IStorageService> _mockStorageService;
        private IStorageService _storageServiceMock;
        private Mock<IRegionManager> _mockRegionManager;
        private IRegionManager _regionManagerMock;
        private Mock<IEventAggregator> _mockEventAggregator;
        private IEventAggregator _eventAggregatorMock;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRegionManager = new Mock<IRegionManager>();
            _regionManagerMock = _mockRegionManager.Object;
            _mockEventAggregator = new Mock<IEventAggregator>();
            _eventAggregatorMock = _mockEventAggregator.Object;
            _mockStorageService = new Mock<IStorageService>();
            _storageServiceMock = _mockStorageService.Object;
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        [TestMethod]
        public void TestConstructorInitializingDTOs()
        {
            var testFarm = new Farm();
            var testDataClassInstance = new DefaultManureCompositionData();
            testDataClassInstance.MoistureContent = 75.0;
            testDataClassInstance.NitrogenFraction = 0.5;
            testDataClassInstance.PhosphorusFraction = 0.3;
            testDataClassInstance.CarbonFraction = 6.0;
            testDataClassInstance.CarbonToNitrogenRatio = 10.0;
            testFarm.DefaultManureCompositionData.Add(testDataClassInstance);
            _mockStorageService.Setup(x => x.GetActiveFarm()).Returns(testFarm);


            _viewModel = new DefaultManureCompositionViewModel(_regionManagerMock, _eventAggregatorMock, _storageServiceMock);

            Assert.AreEqual(1, _viewModel.DefaultManureCompositionDataViewModels.Count);
            Assert.AreEqual(testDataClassInstance.MoistureContent, _viewModel.DefaultManureCompositionDataViewModels[0].MoistureContent);
            Assert.AreEqual(testDataClassInstance.NitrogenFraction, _viewModel.DefaultManureCompositionDataViewModels[0].NitrogenFraction);
            Assert.AreEqual(testDataClassInstance.PhosphorusFraction, _viewModel.DefaultManureCompositionDataViewModels[0].PhosphorusFraction);
            Assert.AreEqual(testDataClassInstance.CarbonFraction, _viewModel.DefaultManureCompositionDataViewModels[0].CarbonFraction);
            Assert.AreEqual(testDataClassInstance.CarbonToNitrogenRatio, _viewModel.DefaultManureCompositionDataViewModels[0].CarbonToNitrogenRatio);
        }
    }
}