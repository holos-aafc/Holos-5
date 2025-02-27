using H.Core.Enumerations;
using H.Core.Models;
using H.Core.Models.Animals;
using H.Core.Models.Animals.OtherAnimals;
using H.Core.Services.StorageService;
using Moq;

namespace H.Avalonia.ViewModels.ComponentViews.OtherAnimals.Tests
{
    [TestClass]
    public class HorsesComponentViewModelTests
    {
        private HorsesComponentViewModel _viewModel;
        private IStorageService _mockStorageService;
        private Mock<IStorageService> _mock;

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
            _mock = new Mock<IStorageService>();
            _mockStorageService = _mock.Object;
            _viewModel = new HorsesComponentViewModel(_mockStorageService);
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        [TestMethod]
        public void TestConstructorSettingViewName()
        {
            string expectedName = "Horses";
            Assert.AreEqual(expectedName, _viewModel.ViewName);
        }

        [TestMethod]
        public void TestConstructorSettingAnimalType()
        {
            AnimalType expectedAnimalType = AnimalType.Horses;
            Assert.AreEqual(expectedAnimalType, _viewModel.OtherAnimalType);
        }

        // Below we are testing methods found in OtherAnimalsViewModelBase (abstract) used by all child classes (horses, bison, goats, etc.)

        [TestMethod]
        public void TestConstructorInitializingCollections()
        {
            Assert.IsNotNull(_viewModel.ManagementPeriodViewModels);
            Assert.AreEqual(0, _viewModel.ManagementPeriodViewModels.Count);
            Assert.IsNotNull(_viewModel.Groups);
            Assert.AreEqual(0, _viewModel.Groups.Count);
        }

        [TestMethod]
        public void TestHandleAddGroupEvent()
        {
            AnimalType expectedGroupType = AnimalType.Horses;

            _viewModel.HandleAddGroupEvent();

            Assert.AreEqual(1, _viewModel.Groups.Count);
            Assert.AreEqual(expectedGroupType, _viewModel.Groups[0].GroupType);
        }

        [TestMethod]
        public void TestHandleAddManagementPeriodEvent()
        {
            string expectedPeriodName = "Period #0";
            DateTime expectedStartDate = new DateTime(2024, 01, 01);
            DateTime expectedEndDate = new DateTime(2025, 01, 01);
            int expectedDays = 364;

            _viewModel.HandleAddManagementPeriodEvent();

            Assert.AreEqual(1, _viewModel.ManagementPeriodViewModels.Count);
            Assert.AreEqual(expectedPeriodName, _viewModel.ManagementPeriodViewModels[0].PeriodName);
            Assert.AreEqual(expectedStartDate, _viewModel.ManagementPeriodViewModels[0].StartDate);
            Assert.AreEqual(expectedEndDate, _viewModel.ManagementPeriodViewModels[0].EndDate);
            Assert.AreEqual(expectedDays, _viewModel.ManagementPeriodViewModels[0].NumberOfDays);
        }

        [TestMethod]
        public void TestAddExistingManagementPeriods()
        {
            var testFarm = new Farm();
            var testHorsesComponent = new HorsesComponent();
            var testGroup = new AnimalGroup();
            var testManagementPeriod = new ManagementPeriod() { GroupName = "Period #0", Start = new DateTime(2020, 01, 01), End = new DateTime(2020, 03, 13), NumberOfDays = 72 };
            testGroup.ManagementPeriods.Add(testManagementPeriod);
            testHorsesComponent.Groups.Add(testGroup);
            testFarm.Components.Add(testHorsesComponent);
            _mock.Setup(x => x.GetActiveFarm()).Returns(testFarm);
            
            
           _viewModel.AddExistingManagementPeriods();

            Assert.AreEqual(testManagementPeriod.GroupName, _viewModel.ManagementPeriodViewModels[0].PeriodName);
            Assert.AreEqual(testManagementPeriod.Start, _viewModel.ManagementPeriodViewModels[0].StartDate);
            Assert.AreEqual(testManagementPeriod.End, _viewModel.ManagementPeriodViewModels[0].EndDate);
            Assert.AreEqual(testManagementPeriod.NumberOfDays, _viewModel.ManagementPeriodViewModels[0].NumberOfDays);
        }

        [TestMethod]
        public void TestValidateViewName()
        {
            Assert.IsFalse(_viewModel.HasErrors);

            _viewModel.ViewName = "";

            Assert.IsTrue(_viewModel.HasErrors);
            var errors = _viewModel.GetErrors(nameof(_viewModel.ViewName)) as IEnumerable<string>;
            Assert.IsNotNull(errors);
            Assert.AreEqual("Name cannot be empty.", errors.ToList()[0]);
        }
    }
}