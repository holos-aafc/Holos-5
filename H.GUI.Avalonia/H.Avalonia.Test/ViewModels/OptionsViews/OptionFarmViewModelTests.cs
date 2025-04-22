using Microsoft.VisualStudio.TestTools.UnitTesting;
using H.Avalonia.ViewModels.OptionsViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H.Core.Services.StorageService;
using Moq;
using Prism.Regions;
using H.Core.Enumerations;
using System.Collections.ObjectModel;
using H.Core.Models;
using H.Core;

namespace H.Avalonia.ViewModels.OptionsViews.Tests
{
    [TestClass]
    public class OptionFarmViewModelTests
    {
        private OptionFarmViewModel _viewModel;
        private Mock<IRegionManager> _mockRegionManager;
        private IRegionManager _regionManagerMock;
        private Mock<IStorageService> _mockStorageService;
        private IStorageService _storageServiceMock;
        private Mock<IStorage> _mockStorage;
        private IStorage _storageMock;

        [ClassCleanup]
        public static void ClassCleanup()
        {
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRegionManager = new Mock<IRegionManager>();
            _regionManagerMock = _mockRegionManager.Object;
            _mockStorageService = new Mock<IStorageService>();
            _storageServiceMock = _mockStorageService.Object;
            _mockStorage = new Mock<IStorage>();
            _storageMock = _mockStorage.Object;
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        [TestMethod]
        public void TestConstructorInitializingProperties()
        {
            var testFarm = new Farm();
            testFarm.Name = "TestFarm";
            testFarm.Comments = "Test Comments";
            testFarm.Latitude = 12.50;
            testFarm.Longitude = 17.82;
            testFarm.ClimateData.PrecipitationData.GrowingSeasonPrecipitation = 50.66;
            testFarm.ClimateData.EvapotranspirationData.GrowingSeasonEvapotranspiration = 125.27;
            testFarm.MeasurementSystemType = MeasurementSystemType.Metric;
            var testMeasurementCollection = new ObservableCollection<MeasurementSystemType>() { MeasurementSystemType.Metric, MeasurementSystemType.Imperial };
            _mockStorageService.Setup(x => x.GetActiveFarm()).Returns(testFarm);

            _viewModel = new OptionFarmViewModel(_regionManagerMock, _storageServiceMock);

            Assert.AreEqual(testFarm.Name, _viewModel.Data.FarmName);
            Assert.AreEqual(testFarm.Comments, _viewModel.Data.FarmComments);
            Assert.AreEqual($"{testFarm.Latitude}, {testFarm.Longitude}", _viewModel.Data.Coordinates);
            Assert.AreEqual(testFarm.ClimateData.PrecipitationData.GrowingSeasonPrecipitation, _viewModel.Data.GrowingSeasonPrecipitation);
            Assert.AreEqual(testFarm.ClimateData.EvapotranspirationData.GrowingSeasonEvapotranspiration, _viewModel.Data.GrowingSeasonEvapotranspiration);
            Assert.AreEqual(testFarm.MeasurementSystemType, _viewModel.SelectedMeasurementSystem);
            Assert.AreEqual(testMeasurementCollection[0], _viewModel.MeasurementSystemTypes[0]);
            Assert.AreEqual(testMeasurementCollection[1], _viewModel.MeasurementSystemTypes[1]);
        }

        [TestMethod]
        public void TestSettingImperial()
        {
            var testFarm = new Farm();
            testFarm.MeasurementSystemType = MeasurementSystemType.Metric;
            testFarm.MeasurementSystemSelected = true;
            var applicationDataInstance = new ApplicationData() { DisplayUnitStrings = new DisplayUnitStrings() };
            
            _mockStorage.Setup(x => x.ApplicationData).Returns(applicationDataInstance);
            _mockStorageService.Setup(x => x.Storage).Returns(_storageMock);
            _mockStorageService.Setup(x => x.GetActiveFarm()).Returns(testFarm);
            _viewModel = new OptionFarmViewModel(_regionManagerMock, _storageServiceMock);

            _viewModel.SelectedMeasurementSystem = MeasurementSystemType.Imperial;

            Assert.AreEqual(testFarm.MeasurementSystemType, MeasurementSystemType.Imperial);
            Assert.IsTrue(testFarm.MeasurementSystemSelected);
        }

        [TestMethod]
        public void TestSettingMetric() 
        {
            var testFarm = new Farm();
            testFarm.MeasurementSystemType = MeasurementSystemType.Imperial;
            testFarm.MeasurementSystemSelected = true;
            var applicationDataInstance = new ApplicationData() { DisplayUnitStrings = new DisplayUnitStrings() };

            _mockStorage.Setup(x => x.ApplicationData).Returns(applicationDataInstance);
            _mockStorageService.Setup(x => x.Storage).Returns(_storageMock);
            _mockStorageService.Setup(x => x.GetActiveFarm()).Returns(testFarm);
            _viewModel = new OptionFarmViewModel(_regionManagerMock, _storageServiceMock);

            _viewModel.SelectedMeasurementSystem = MeasurementSystemType.Metric;

            Assert.AreEqual(testFarm.MeasurementSystemType, MeasurementSystemType.Metric);
            Assert.IsTrue(testFarm.MeasurementSystemSelected);
        }
    }
}