using System.Collections.ObjectModel;
using H.Core.Calculators.UnitsOfMeasurement;
using H.Core.Enumerations;
using H.Core.Factories;
using H.Core.Models.LandManagement.Fields;
using H.Core.Services.LandManagement;
using H.Core.Services.LandManagement.Fields;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace H.Core.Test;

[TestClass]
public class FieldComponentServiceTest
{
    #region Fields

    private IFieldComponentService _fieldComponentService;
    private Mock<IFieldComponentDtoFactory> _mockFieldComponentDtoFactory;
    private Mock<IUnitsOfMeasurementCalculator> _mockUnitsOfMeasurementCalculator;

    #endregion

    #region Initialization

    [ClassInitialize]
    public static void ClassInitialize(TestContext testContext)
    {
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
    }

    [TestInitialize]
    public void TestInitialize()
    {
        _mockFieldComponentDtoFactory = new Mock<IFieldComponentDtoFactory>();
        var mockCropDtoFactory = new Mock<ICropDtoFactory>();
        _mockUnitsOfMeasurementCalculator = new Mock<IUnitsOfMeasurementCalculator>();

        _fieldComponentService = new FieldComponentService(_mockFieldComponentDtoFactory.Object, mockCropDtoFactory.Object, _mockUnitsOfMeasurementCalculator.Object);
    }

    [TestCleanup]
    public void TestCleanup()
    {
    }

    #endregion

    #region Tests

    [TestMethod]
    public void TransferToSystemConvertsImperialValueToMetric()
    {
        // Display units are imperial
        _mockUnitsOfMeasurementCalculator.Setup(x => x.GetUnitsOfMeasurement()).Returns(MeasurementSystemType.Imperial);

        const double areaInAcres = 20;
        
        var dto = new FieldSystemComponentDto();

        // User sets field area to 20 acres
        dto.FieldArea = areaInAcres;

        _mockFieldComponentDtoFactory.Setup(x => x.Create(It.IsAny<IFieldComponentDto>())).Returns(dto);

        var fieldComponent = new FieldSystemComponent();

        // We need to ensure the DTO value of 20 acres gets converted to hectares and assigned to the system/domain object
        var result = _fieldComponentService.TransferToSystem(dto, fieldComponent);

        var expected = areaInAcres / 2.4711;

        Assert.AreEqual(expected, result.FieldArea);
    }

    [TestMethod]
    public void TransferToSystemConvertsToMetric()
    {
        // Display units are metric
        _mockUnitsOfMeasurementCalculator.Setup(x => x.GetUnitsOfMeasurement()).Returns(MeasurementSystemType.Metric);

        const double areaInHectares = 20;

        var dto = new FieldSystemComponentDto();

        // User sets field area to 20 hectares
        dto.FieldArea = areaInHectares;

        _mockFieldComponentDtoFactory.Setup(x => x.Create(It.IsAny<IFieldComponentDto>())).Returns(dto);

        var fieldComponent = new FieldSystemComponent();

        // We need to ensure the DTO value of 20 hectares gets converted to hectares (this is the complementary test to the one above) and assigned to the system/domain object
        var result = _fieldComponentService.TransferToSystem(dto, fieldComponent);

        var expected = areaInHectares;

        Assert.AreEqual(expected, result.FieldArea);
    }

    [TestMethod]
    public void CreateSetCropDtoCollectionToNonEmpty()
    {
        var result = _fieldComponentService.TransferToFieldComponentDto(new FieldSystemComponent() { CropViewItems = new ObservableCollection<CropViewItem>() { new CropViewItem() } });

        Assert.IsTrue(result.CropDtos.Any());
    }

    [TestMethod]
    public void CreateSetCropDtoCollectionToEmpty()
    {
        var result = _fieldComponentService.TransferToFieldComponentDto(new FieldSystemComponent() { CropViewItems = new ObservableCollection<CropViewItem>() { } });

        Assert.IsFalse(result.CropDtos.Any());
    }

    #endregion
}
