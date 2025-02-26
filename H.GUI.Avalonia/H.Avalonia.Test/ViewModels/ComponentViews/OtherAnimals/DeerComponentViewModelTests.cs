using H.Core.Enumerations;
using H.Core.Services.StorageService;
using Moq;

namespace H.Avalonia.ViewModels.ComponentViews.OtherAnimals.Tests
{
    [TestClass]
    public class DeerComponentViewModelTests
    {
        private DeerComponentViewModel _viewModel;
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
            _viewModel = new DeerComponentViewModel(_mockStorageService);
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        [TestMethod]
        public void TestConstructorSettingViewName()
        {
            string expectedName = "Deer";
            Assert.AreEqual(expectedName, _viewModel.ViewName);
        }

        [TestMethod]
        public void TestConstructorSettingAnimalType()
        {
            AnimalType expectedAnimalType = AnimalType.Deer;
            Assert.AreEqual(expectedAnimalType, _viewModel.OtherAnimalType);
        }
    }
}