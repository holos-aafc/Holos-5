using Microsoft.VisualStudio.TestTools.UnitTesting;
using H.Avalonia.ViewModels.OptionsViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H.Core.Models;
using H.Core.Providers.Temperature;
using H.Core.Services.StorageService;
using H.Core;
using Moq;
using H.Core.Providers.Evapotranspiration;

namespace H.Avalonia.ViewModels.OptionsViews.Tests
{
    [TestClass]
    public class EvapotranspirationSettingsViewModelTests
    {
        private Farm _testFarm;
        private EvapotranspirationData _evapotranspirationData;
        private EvapotranspirationSettingsViewModel _viewModel;
        private Mock<IStorageService> _mockStorageService;
        private IStorageService _storageServiceMock;
        private Mock<IStorage> _mockStorage;
        private IStorage _storageMock;
        private ApplicationData _applicationData;

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

            _applicationData = new ApplicationData();
            _mockStorage.Setup(x => x.ApplicationData).Returns(_applicationData);
            _mockStorageService.Setup(x => x.Storage).Returns(_storageMock);

            _testFarm = new Farm();
            _evapotranspirationData = new EvapotranspirationData
            {
                January = 2.1,
                February = 4.3,
                March = 1.5,
                April = 3.5,
                May = 5.0,
                June = 10.2,
                July = 6.6,
                August = 13.2,
                September = 4.4,
                October = 8.8,
                November = 7.7,
                December = 9.4
            };
            _testFarm.ClimateData.EvapotranspirationData = _evapotranspirationData;
            _mockStorageService.Setup(x => x.GetActiveFarm()).Returns(_testFarm);
        }

        [TestMethod]
        public void TestInitializationLogic()
        {
            _viewModel = new EvapotranspirationSettingsViewModel(_storageServiceMock);

            Assert.AreEqual(2.1, _viewModel.Data.January);
            Assert.AreEqual(4.3, _viewModel.Data.February);
            Assert.AreEqual(1.5, _viewModel.Data.March);
            Assert.AreEqual(3.5, _viewModel.Data.April);
            Assert.AreEqual(5.0, _viewModel.Data.May);
            Assert.AreEqual(10.2, _viewModel.Data.June);
            Assert.AreEqual(6.6, _viewModel.Data.July);
            Assert.AreEqual(13.2, _viewModel.Data.August);
            Assert.AreEqual(4.4, _viewModel.Data.September);
            Assert.AreEqual(8.8, _viewModel.Data.October);
            Assert.AreEqual(7.7, _viewModel.Data.November);
            Assert.AreEqual(9.4, _viewModel.Data.December);
        }

        [TestMethod]
        public void TestConstructuroThrowsExceptionOnNullConstructorParameter()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new EvapotranspirationSettingsViewModel(null));
        }
    }
}