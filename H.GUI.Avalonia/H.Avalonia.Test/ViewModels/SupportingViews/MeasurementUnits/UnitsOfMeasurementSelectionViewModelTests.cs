using System.Collections.ObjectModel;
using H.Core.Calculators.UnitsOfMeasurement;
using H.Core.Enumerations;
using H.Core.Models;
using H.Core.Services.StorageService;
using Moq;
using Prism.Events;
using Prism.Regions;

namespace H.Avalonia.ViewModels.SupportingViews.MeasurementUnits.Tests
{
    [TestClass]
    public class UnitsOfMeasurementSelectionViewModelTests
    {
        private UnitsOfMeasurementSelectionViewModel _viewModel;
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
        public void TestConstructor()
        {
            _viewModel = new UnitsOfMeasurementSelectionViewModel(_regionManagerMock, _eventAggregatorMock, _storageServiceMock);
            var expectedCollection = new ObservableCollection<MeasurementSystemType>() { MeasurementSystemType.None, MeasurementSystemType.Metric, MeasurementSystemType.Imperial };
            var expectedSelectedMeasurementType = MeasurementSystemType.None;
            Assert.IsNotNull(_viewModel);
            Assert.AreEqual(expectedCollection[0], _viewModel.MeasurementSystemTypes[0]);
            Assert.AreEqual(expectedCollection[1], _viewModel.MeasurementSystemTypes[1]);
            Assert.AreEqual(expectedCollection[2], _viewModel.MeasurementSystemTypes[2]);
            Assert.AreEqual(expectedSelectedMeasurementType, _viewModel.SelectedMeasurementSystem);
        }

        [TestMethod]
        public void TestSettingMetric()
        {
            var testFarm = new Farm();
            Assert.IsFalse(testFarm.MeasurementSystemSelected);
            _mockStorageService.Setup(x => x.GetActiveFarm()).Returns(testFarm);
            _viewModel = new UnitsOfMeasurementSelectionViewModel(_regionManagerMock, _eventAggregatorMock, _storageServiceMock);
            Assert.AreEqual(MeasurementSystemType.None, _viewModel.SelectedMeasurementSystem);

            _viewModel.SelectedMeasurementSystem = MeasurementSystemType.Metric;

            Assert.AreEqual(MeasurementSystemType.Metric, _viewModel.SelectedMeasurementSystem);
            Assert.AreEqual(testFarm.MeasurementSystemType, _viewModel.SelectedMeasurementSystem);
            Assert.IsTrue(testFarm.MeasurementSystemSelected);
        }

        [TestMethod] 
        public void TestSettingImperial()
        {
            var testFarm = new Farm();
            Assert.IsFalse(testFarm.MeasurementSystemSelected);
            _mockStorageService.Setup(x => x.GetActiveFarm()).Returns(testFarm);
            _viewModel = new UnitsOfMeasurementSelectionViewModel(_regionManagerMock, _eventAggregatorMock, _storageServiceMock);
            Assert.AreEqual(MeasurementSystemType.None, _viewModel.SelectedMeasurementSystem);

            _viewModel.SelectedMeasurementSystem = MeasurementSystemType.Imperial;

            Assert.AreEqual(MeasurementSystemType.Imperial, _viewModel.SelectedMeasurementSystem);
            Assert.AreEqual(testFarm.MeasurementSystemType, _viewModel.SelectedMeasurementSystem);
            Assert.IsTrue(testFarm.MeasurementSystemSelected);
        }
    }
}