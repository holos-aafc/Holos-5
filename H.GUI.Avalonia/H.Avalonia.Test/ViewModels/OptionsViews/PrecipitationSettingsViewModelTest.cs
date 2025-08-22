using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H.Avalonia.ViewModels.OptionsViews;
using H.Core.Models;
using H.Core.Services.StorageService;
using H.Core;
using H.Core.Providers.Climate;
using H.Core.Providers.Precipitation;
using Moq;
using Prism.Regions;
using Mapsui;
using H.Core.Enumerations;

namespace H.Avalonia.Test.ViewModels.OptionsViews.Tests
{
    [TestClass]
    public class PrecipitationSettingsViewModelTests
    {
        private Farm _testFarm;
        private ClimateData _climateData;
        private PrecipitationData _precipitationData;
        private PrecipitationSettingsViewModel _viewModel;
        private Mock<IStorageService> _mockStorageService;
        private IStorageService _storageServiceMock;
        private Mock<IStorage> _mockStorage;
        private IStorage _storageMock;
        private ApplicationData _applicationData;

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

            _applicationData = new ApplicationData() { GlobalSettings = new GlobalSettings() };
            _mockStorage.Setup(x => x.ApplicationData).Returns(_applicationData);
            _mockStorageService.Setup(x => x.Storage).Returns(_storageMock);

            _testFarm = new Farm();
            _climateData = new ClimateData();
            _precipitationData = new PrecipitationData
            {
                January = 1.0,
                February = 2.0,
                March = 3.0,
                April = 4.0,
                May = 5.0,
                June = 6.0,
                July = 7.0,
                August = 8.0,
                September = 9.0,
                October = 10.0,
                November = 11.0,
                December = 12.0
            };
            _climateData.PrecipitationData = _precipitationData;
            _testFarm.ClimateData = _climateData;

            _applicationData.GlobalSettings.ActiveFarm = _testFarm;
            _storageServiceMock.AddFarm(_testFarm);
            _storageServiceMock.SetActiveFarm(_testFarm);

            _viewModel = new PrecipitationSettingsViewModel(_storageServiceMock);
            
        }
    }
}
