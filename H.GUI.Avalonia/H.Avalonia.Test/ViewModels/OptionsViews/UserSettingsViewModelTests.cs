using Microsoft.VisualStudio.TestTools.UnitTesting;
using H.Avalonia.ViewModels.OptionsViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H.Core.Models;
using H.Core.Services.StorageService;
using H.Core;
using Moq;

namespace H.Avalonia.ViewModels.OptionsViews.Tests
{
    [TestClass]
    public class UserSettingsViewModelTests
    {
        private Mock<IStorageService> _mockStorageService;
        private IStorageService _storageServiceMock;
        private Mock<IStorage> _mockStorage;
        private IStorage _storageMock;
        private ApplicationData _applicationData;
        private UserSettingsViewModel _userSettingsViewModel;

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
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        [TestMethod]
        public void TestDataInitializationLogic()
        {
            var activeFarm = new Farm() { Name = "TestFarm" };
            activeFarm.Defaults.CustomN2OEmissionFactor = 1.12;
            activeFarm.Defaults.EmissionFactorForLeachingAndRunoff = 3.75;
            activeFarm.Defaults.PercentageOfStrawReturnedToSoilForRootCrops = 44.67;
            activeFarm.Defaults.PercentageOfRootsReturnedToSoilForFodderCorn = 94.34;
            activeFarm.Defaults.EquilibriumCalculationStrategy = H.Core.Enumerations.EquilibriumCalculationStrategies.CarSingleYear;
            _mockStorageService.Setup(x => x.GetActiveFarm()).Returns(activeFarm);

            _userSettingsViewModel = new UserSettingsViewModel(_storageServiceMock); // Ctor calls Initialize() method

            Assert.IsNotNull(_userSettingsViewModel);
            Assert.AreEqual(1.12, _userSettingsViewModel.Data.CustomN2OEmissionFactor);
            Assert.AreEqual(3.75, _userSettingsViewModel.Data.EmissionFactorForLeachingAndRunoff);
            Assert.AreEqual(44.67, _userSettingsViewModel.Data.PercentageOfStrawReturnedToSoilForRootCrops);
            Assert.AreEqual(94.34, _userSettingsViewModel.Data.PercentageOfRootsReturnedToSoilForFodderCorn);
            Assert.AreEqual(H.Core.Enumerations.EquilibriumCalculationStrategies.CarSingleYear, _userSettingsViewModel.Data.EquilibriumCalculationStrategy);
        }
    }
}