using H.Core.Enumerations;

namespace H.Avalonia.ViewModels.ComponentViews.OtherAnimals.Tests
{
    [TestClass]
    public class LlamaComponentViewModelTests
    {
        private static LlamaComponentViewModel _viewModel;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _viewModel = new LlamaComponentViewModel();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
        }

        [TestMethod]
        public void TestConstructorSettingViewName()
        {
            string expectedName = "Llamas";
            Assert.AreEqual(expectedName, _viewModel.ViewName);
        }

        [TestMethod]
        public void TestConstructorSettingAnimalType()
        {
            AnimalType expectedAnimalType = AnimalType.Llamas;
            Assert.AreEqual(expectedAnimalType, _viewModel.OtherAnimalType);
        }
    }
}