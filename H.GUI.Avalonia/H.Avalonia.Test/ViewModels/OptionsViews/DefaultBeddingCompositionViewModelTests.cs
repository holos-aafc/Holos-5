using H.Core;
using H.Core.Calculators.UnitsOfMeasurement;
using H.Core.Models;
using H.Core.Providers.Animals;
using H.Core.Services.StorageService;
using H.Core.Enumerations;
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

        private Mock<IStorage> _mockStorage;
        private IStorage _storageMock;

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
            
            _mockStorage = new Mock<IStorage>();
            _storageMock = _mockStorage.Object;
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

            var displayUnitsInstance = new DisplayUnitStrings();
            displayUnitsInstance.SetStrings(MeasurementSystemType.Metric);
            var applicationDataInstance = new ApplicationData();
            applicationDataInstance.DisplayUnitStrings = displayUnitsInstance;

            _mockStorage.Setup(x => x.ApplicationData).Returns(applicationDataInstance);
            _mockStorageService.Setup(x => x.Storage).Returns(_storageMock);
            _mockStorageService.Setup(x => x.GetActiveFarm()).Returns(testFarm);
            _mockUnitsCalculator.Setup(x => x.IsMetric).Returns(true);

            _viewModel = new DefaultBeddingCompositionViewModel(_regionManagerMock, _eventAggregatorMock, _storageServiceMock, _unitsCalculatorMock);

            Assert.AreEqual(1, _viewModel.BeddingCompositionDataViewModels.Count);
            Assert.AreEqual(testDataClassInstance.TotalNitrogenKilogramsDryMatter, _viewModel.BeddingCompositionDataViewModels[0].TotalNitrogenKilogramsDryMatter);
            Assert.AreEqual(testDataClassInstance.TotalPhosphorusKilogramsDryMatter, _viewModel.BeddingCompositionDataViewModels[0].TotalPhosphorusKilogramsDryMatter);
            Assert.AreEqual(testDataClassInstance.TotalCarbonKilogramsDryMatter, _viewModel.BeddingCompositionDataViewModels[0].TotalCarbonKilogramsDryMatter);
            Assert.AreEqual(testDataClassInstance.CarbonToNitrogenRatio, _viewModel.BeddingCompositionDataViewModels[0].CarbonToNitrogenRatio);
        }

        [TestMethod]
        public void TestSetHeaderStringsMetric()
        {
            // Set up to mirror logic when the user presses the next button in the UnitsOfMeasurementSelectionView/Model
            var testFarm = new Farm();
            testFarm.MeasurementSystemType = MeasurementSystemType.Metric;
            testFarm.MeasurementSystemSelected = true;
            var displayUnitsInstance = new DisplayUnitStrings();
            displayUnitsInstance.SetStrings(testFarm.MeasurementSystemType);
            var applicationDataInstance = new ApplicationData();
            applicationDataInstance.DisplayUnitStrings = displayUnitsInstance;

            _mockStorage.Setup(x => x.ApplicationData).Returns(applicationDataInstance);
            _mockStorageService.Setup(x => x.Storage).Returns(_storageMock);
            _mockStorageService.Setup(x => x.GetActiveFarm()).Returns(testFarm);

            // Constructor calls private SetHeaderStrings() method
            _viewModel = new DefaultBeddingCompositionViewModel(_regionManagerMock, _eventAggregatorMock, _storageServiceMock, _unitsCalculatorMock);

            Assert.AreEqual("Total nitrogen (kg N (kg DM)^1)", _viewModel.NitrogenConcentrationHeader);
            Assert.AreEqual("Total phosphorus (kg P (kg DM)^1)", _viewModel.PhosphorusConcentrationHeader);
            Assert.AreEqual("Total carbon (kg C (kg DM)^1)", _viewModel.CarbonConcentrationHeader);
        }

        [TestMethod]
        public void TestSetHeaderStringsImperial()
        {
            // Set up to mirror logic when the user presses the next button in the UnitsOfMeasurementSelectionView/Model
            var testFarm = new Farm();
            testFarm.MeasurementSystemType = MeasurementSystemType.Imperial;
            testFarm.MeasurementSystemSelected = true;
            var displayUnitsInstance = new DisplayUnitStrings();
            displayUnitsInstance.SetStrings(testFarm.MeasurementSystemType);
            var applicationDataInstance = new ApplicationData();
            applicationDataInstance.DisplayUnitStrings = displayUnitsInstance;

            _mockStorage.Setup(x => x.ApplicationData).Returns(applicationDataInstance);
            _mockStorageService.Setup(x => x.Storage).Returns(_storageMock);
            _mockStorageService.Setup(x => x.GetActiveFarm()).Returns(testFarm);

            // Constructor calls private SetHeaderStrings() method
            _viewModel = new DefaultBeddingCompositionViewModel(_regionManagerMock, _eventAggregatorMock, _storageServiceMock, _unitsCalculatorMock);

            Assert.AreEqual("Total nitrogen (lb N (lb DM)^-1)", _viewModel.NitrogenConcentrationHeader);
            Assert.AreEqual("Total phosphorus (lb P (lb DM)^-1)", _viewModel.PhosphorusConcentrationHeader);
            Assert.AreEqual("Total carbon (lb C (lb DM)^-1)", _viewModel.CarbonConcentrationHeader);
        }
    }
}