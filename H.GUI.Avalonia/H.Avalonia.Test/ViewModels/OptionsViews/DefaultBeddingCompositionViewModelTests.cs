using DynamicData;
using H.Core.Calculators.UnitsOfMeasurement;
using H.Core.Models;
using H.Core.Providers.Animals;
using H.Core.Services.StorageService;
using Moq;
using Prism.Events;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews.Tests
{
    [TestClass]
    public class DefaultBeddingCompositionViewModelTests
    {
        private DefaultBeddingCompositionViewModel _viewModel;
        private Mock<IStorageService> _mockStorageService;
        private IStorageService _storageServiceMock;
        private Mock<IRegionManager> _mockRegionManager;
        private IRegionManager _regionManagerMock;
        private Mock<IEventAggregator> _mockEventAggregator;
        private IEventAggregator _eventAggregatorMock;
        private Mock<IUnitsOfMeasurementCalculator> _mockUnitsCalculator;
        private IUnitsOfMeasurementCalculator _unitsCalculatorMock;

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
            _mockUnitsCalculator = new Mock<IUnitsOfMeasurementCalculator>();
            _unitsCalculatorMock = _mockUnitsCalculator.Object;
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        [TestMethod]
        public void TestConstructorInitializingDTOs()
        {
            var testFarm = new Farm();
            var testDataClassInstance = new Table_30_Default_Bedding_Material_Composition_Data();
            testDataClassInstance.TotalNitrogenKilogramsDryMatter = 0.005;
            testDataClassInstance.TotalPhosphorusKilogramsDryMatter = 0.001;
            testDataClassInstance.TotalCarbonKilogramsDryMatter = 0.3;
            testDataClassInstance.CarbonToNitrogenRatio = 50.0;
            testFarm.DefaultsCompositionOfBeddingMaterials.Add(testDataClassInstance);
            _mockStorageService.Setup(x => x.GetActiveFarm()).Returns(testFarm);
            _mockUnitsCalculator.Setup(x => x.IsMetric).Returns(true);

            _viewModel = new DefaultBeddingCompositionViewModel(_regionManagerMock, _eventAggregatorMock, _storageServiceMock, _unitsCalculatorMock);

            Assert.AreEqual(1, _viewModel.BeddingCompositionDataViewModels.Count);
            Assert.AreEqual(testDataClassInstance.TotalNitrogenKilogramsDryMatter, _viewModel.BeddingCompositionDataViewModels[0].TotalNitrogenKilogramsDryMatter);
            Assert.AreEqual(testDataClassInstance.TotalPhosphorusKilogramsDryMatter, _viewModel.BeddingCompositionDataViewModels[0].TotalPhosphorusKilogramsDryMatter);
            Assert.AreEqual(testDataClassInstance.TotalCarbonKilogramsDryMatter, _viewModel.BeddingCompositionDataViewModels[0].TotalCarbonKilogramsDryMatter);
            Assert.AreEqual(testDataClassInstance.CarbonToNitrogenRatio, _viewModel.BeddingCompositionDataViewModels[0].CarbonToNitrogenRatio);
        }
    }
}