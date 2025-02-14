using H.Core.Enumerations;

namespace H.Avalonia.ViewModels.ComponentViews.OtherAnimals.Tests
{
    [TestClass]
    public class OtherAnimalsViewModelBaseTests
    {
        private static OtherAnimalsViewModelBase _viewModel;
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _viewModel = new OtherAnimalsViewModelBase();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
        } 

        [TestMethod]
        public void TestConstructorInitializingCollections()
        {
            Assert.IsNotNull(_viewModel.ManagementPeriods);
            Assert.AreEqual(0, _viewModel.ManagementPeriods.Count);
            Assert.IsNotNull(_viewModel.Groups);
            Assert.AreEqual(0, _viewModel.Groups.Count);
        }

        [TestMethod]
        public void OnNavigatedToTest()
        {
        }

        [TestMethod]
        public void TestHandleAddGroupEvent()
        {
            AnimalType expectedGroupType = AnimalType.NotSelected;
            
            _viewModel.HandleAddGroupEvent();

            Assert.AreEqual(1, _viewModel.Groups.Count);
            Assert.AreEqual(expectedGroupType, _viewModel.Groups[0].GroupType);
        }

        [TestMethod]
        public void TestHandleAddManagementPeriodEvent()
        {
            string expectedGroupName = "Period #0";
            DateTime expectedStartDate = new DateTime(2024, 01, 01);
            DateTime expectedEndDate = new DateTime(2025, 01, 01);
            int expectedDays = 364;
            
            _viewModel.HandleAddManagementPeriodEvent();
            
            Assert.AreEqual(1, _viewModel.ManagementPeriods.Count);
            Assert.AreEqual(expectedGroupName, _viewModel.ManagementPeriods[0].GroupName);
            Assert.AreEqual(expectedStartDate, _viewModel.ManagementPeriods[0].Start);
            Assert.AreEqual(expectedEndDate, _viewModel.ManagementPeriods[0].End);
            Assert.AreEqual(expectedDays, _viewModel.ManagementPeriods[0].NumberOfDays);
        }
    }
}