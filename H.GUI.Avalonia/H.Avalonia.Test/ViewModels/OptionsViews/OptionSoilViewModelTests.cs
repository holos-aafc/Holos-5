using H.Avalonia.ViewModels.OptionsViews;
using H.Core;
using H.Core.Models;
using H.Core.Services.StorageService;
using Moq;
using Prism.Regions;

namespace H.Avalonia.Test.ViewModels.OptionsViews
{
    [TestClass]
    public class OptionSoilViewModelTests
    {
        private SoilSettingsViewModel _viewModel;
        private Mock<IRegionManager> _mockRegionManager;
        private IRegionManager _regionManagerMock;
        private Mock<IStorageService> _mockStorageService;
        private IStorageService _storageServiceMock;
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

            _mockStorageService = new Mock<IStorageService>();
            _storageServiceMock = _mockStorageService.Object;

            _mockStorage = new Mock<IStorage>();
            _storageMock = _mockStorage.Object;

            var globalSettings = new GlobalSettings();
            var applicationData = new ApplicationData();
            applicationData.GlobalSettings = globalSettings;

            _mockStorage.Setup(x => x.ApplicationData).Returns(applicationData);
            _mockStorageService.Setup(x => x.Storage).Returns(_storageMock);
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
            _mockStorageService.Setup(x => x.GetActiveFarm()).Returns(testFarm);
            _viewModel = new SoilSettingsViewModel(_storageServiceMock);
            Assert.IsNotNull(_viewModel);
            Assert.IsNotNull(_viewModel.Data);
            Assert.IsNotNull(_viewModel.Data.BindingSoilData);
        }

        [TestMethod]
        public void TestConstructuroThrowsExceptionOnNullConstructorParameter()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new SoilSettingsViewModel(null));
        }

    }
}
