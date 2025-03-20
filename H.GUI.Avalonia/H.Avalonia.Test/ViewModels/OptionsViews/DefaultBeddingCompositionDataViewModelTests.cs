using H.Core.Calculators.UnitsOfMeasurement;
using Moq;
using H.Core.Providers.Animals;
using H.Core.Enumerations;

namespace H.Avalonia.ViewModels.OptionsViews.Tests
{
    [TestClass]
    public class DefaultBeddingCompositionDataViewModelTests
    {
        private DefaultBeddingCompositionDataViewModel _viewModel;
        private Mock<IUnitsOfMeasurementCalculator> _mockUnitsCalculator;
        private IUnitsOfMeasurementCalculator _unitsCalculatorMock;

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
            _mockUnitsCalculator = new Mock<IUnitsOfMeasurementCalculator>();
            _unitsCalculatorMock = _mockUnitsCalculator.Object;
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        [TestMethod]
        public void TestGetterAndSetterWhenMetric()
        {
            _mockUnitsCalculator.Setup(x => x.IsMetric).Returns(true);
            var testDataClassInstance = new Table_30_Default_Bedding_Material_Composition_Data();

            _viewModel = new DefaultBeddingCompositionDataViewModel(testDataClassInstance, _unitsCalculatorMock);
            _viewModel.TotalNitrogenKilogramsDryMatter = 0.005;

            // Getter should return backing field as is (i.e. in metric units)
            Assert.AreEqual(0.005, _viewModel.TotalNitrogenKilogramsDryMatter);
            // Setter should store in metric units / update data class instance properly
            Assert.AreEqual(0.005, testDataClassInstance.TotalNitrogenKilogramsDryMatter);
        }

        [TestMethod]
        public void TestGetterAndSetterWhenImperial()
        {
            _mockUnitsCalculator.Setup(x => x.IsMetric).Returns(false);
            _mockUnitsCalculator.Setup(x => x.GetUnitsOfMeasurementValue(MeasurementSystemType.Metric, ImperialUnitsOfMeasurement.Pounds, 10)).Returns(4.535147);
            _mockUnitsCalculator.Setup(x => x.GetUnitsOfMeasurementValue(MeasurementSystemType.Imperial, MetricUnitsOfMeasurement.Kilograms, 4.535147)).Returns(10);
            var testDataClassInstance = new Table_30_Default_Bedding_Material_Composition_Data();
            _viewModel = new DefaultBeddingCompositionDataViewModel(testDataClassInstance, _unitsCalculatorMock);

            _viewModel.TotalNitrogenKilogramsDryMatter = 10;

            // Getter should return backing field in imperial units (matching input)
            Assert.AreEqual(10, _viewModel.TotalNitrogenKilogramsDryMatter);
            // Setter should store input (and update data class instance) in metric units 
            Assert.AreEqual(4.535147, testDataClassInstance.TotalNitrogenKilogramsDryMatter);
        }

        [TestMethod]
        public void TestValidateNumericProperty()
        {
            _mockUnitsCalculator.Setup(x => x.IsMetric).Returns(true);
            var testDataClassInstance = new Table_30_Default_Bedding_Material_Composition_Data();
            _viewModel = new DefaultBeddingCompositionDataViewModel(testDataClassInstance, _unitsCalculatorMock);
            // Initially set a property to a valid value
            _viewModel.TotalPhosphorusKilogramsDryMatter = 2;
            Assert.IsTrue(!_viewModel.HasErrors);

            _viewModel.TotalPhosphorusKilogramsDryMatter = -7;
            
            // Data class instance should not be updated with invalid values
            Assert.AreEqual(2, testDataClassInstance.TotalPhosphorusKilogramsDryMatter);
            Assert.IsTrue(_viewModel.HasErrors);
            var errors = _viewModel.GetErrors(nameof(_viewModel.TotalPhosphorusKilogramsDryMatter)) as IEnumerable<string>;
            Assert.IsNotNull(errors);
            Assert.AreEqual("Must be greater than or equal to 0.", errors.ToList()[0]);
        }
    }
}