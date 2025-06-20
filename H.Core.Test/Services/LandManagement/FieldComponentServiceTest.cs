using System.Collections.ObjectModel;
using System.Configuration;
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
    private Mock<ICropDtoFactory> _mockCropDtoFactory;
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
        _mockCropDtoFactory = new Mock<ICropDtoFactory>();
        _mockUnitsOfMeasurementCalculator = new Mock<IUnitsOfMeasurementCalculator>();

        _fieldComponentService = new FieldComponentService(_mockFieldComponentDtoFactory.Object, _mockCropDtoFactory.Object, _mockUnitsOfMeasurementCalculator.Object);
    }

    [TestCleanup]
    public void TestCleanup()
    {
    }

    #endregion

    #region Tests

    [TestMethod]
    public void TransferCropDtoToSystemUsingMetricSetsCorrectValue()
    {
        // Display units are metric
        _mockUnitsOfMeasurementCalculator.Setup(x => x.GetUnitsOfMeasurement()).Returns(MeasurementSystemType.Metric);

        var cropViewItem = new CropViewItem()
        {
            AmountOfIrrigation = 100,
        };


        var result = _fieldComponentService.TransferCropViewItemToCropDto(cropViewItem);

        Assert.AreEqual(100, result.AmountOfIrrigation);
    }

    [TestMethod]
    public void TransferCropDtoToSystemUsingImperialSetsCorrectValue()
    {
        // Display units are imperial
        _mockUnitsOfMeasurementCalculator.Setup(x => x.GetUnitsOfMeasurement()).Returns(MeasurementSystemType.Imperial);

        var cropViewItem = new CropViewItem()
        {
            AmountOfIrrigation = 100,
        };

        var result = _fieldComponentService.TransferCropViewItemToCropDto(cropViewItem);

        // Convert 100 millimeters to inches
        var expected = 100 / 25.4;

        Assert.AreEqual(expected, result.AmountOfIrrigation, 0.01);
    }

    [TestMethod]
    public void TransferCropDtoToSystemConvertsImperialValueToMetric()
    {
        // Display units are imperial
        _mockUnitsOfMeasurementCalculator.Setup(x => x.GetUnitsOfMeasurement()).Returns(MeasurementSystemType.Imperial);

        var dto = new CropDto();

        // User sets total annual irrigation to 50 inches
        dto.AmountOfIrrigation = 50;

        _mockCropDtoFactory.Setup(x => x.CreateCropDto(It.IsAny<ICropDto>())).Returns(dto);

        var cropViewItem = new CropViewItem();

        // We need to ensure the DTO value of 50 inches gets converted to millimeters and assigned to the system/domain object
        var result = _fieldComponentService.TransferCropDtoToSystem(dto, cropViewItem);

        var expected = 50 / 0.0394;

        Assert.AreEqual(expected, result.AmountOfIrrigation);
    }

    [TestMethod]
    public void TransferFieldDtoToSystemConvertsImperialValueToMetric()
    {
        // Display units are imperial
        _mockUnitsOfMeasurementCalculator.Setup(x => x.GetUnitsOfMeasurement()).Returns(MeasurementSystemType.Imperial);

        const double areaInAcres = 20;
        
        var dto = new FieldSystemComponentDto();

        // User sets field area to 20 acres
        dto.FieldArea = areaInAcres;

        _mockFieldComponentDtoFactory.Setup(x => x.CreateFieldDto(It.IsAny<IFieldComponentDto>())).Returns(dto);

        var fieldComponent = new FieldSystemComponent();

        // We need to ensure the DTO value of 20 acres gets converted to hectares and assigned to the system/domain object
        var result = _fieldComponentService.TransferFieldDtoToSystem(dto, fieldComponent);

        var expected = areaInAcres / 2.4711;

        Assert.AreEqual(expected, result.FieldArea);
    }

    [TestMethod]
    public void TransferFieldDtoToSystemConvertsToMetric()
    {
        // Display units are metric
        _mockUnitsOfMeasurementCalculator.Setup(x => x.GetUnitsOfMeasurement()).Returns(MeasurementSystemType.Metric);

        const double areaInHectares = 20;

        var dto = new FieldSystemComponentDto();

        // User sets field area to 20 hectares
        dto.FieldArea = areaInHectares;

        _mockFieldComponentDtoFactory.Setup(x => x.CreateFieldDto(It.IsAny<IFieldComponentDto>())).Returns(dto);

        var fieldComponent = new FieldSystemComponent();

        // We need to ensure the DTO value of 20 hectares gets converted to hectares (this is the complementary test to the one above) and assigned to the system/domain object
        var result = _fieldComponentService.TransferFieldDtoToSystem(dto, fieldComponent);

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

    [TestMethod]
    public void BuildCropDtoCollectionDoesNotCreateAnyItems()
    {
        var fieldComponentDto = new FieldSystemComponentDto();
        var fieldComponent = new FieldSystemComponent();

        _fieldComponentService.ConvertCropViewItemsToDtoCollection(fieldComponent, fieldComponentDto);

        Assert.IsFalse(fieldComponentDto.CropDtos.Any());
    }

    [TestMethod]
    public void BuildCropDtoCollectionDoesNotCreatesItems()
    {
        var fieldComponentDto = new FieldSystemComponentDto();
        var fieldComponent = new FieldSystemComponent() {CropViewItems = new ObservableCollection<CropViewItem>() {new CropViewItem()}};

        _fieldComponentService.ConvertCropViewItemsToDtoCollection(fieldComponent, fieldComponentDto);

        Assert.AreEqual(1, fieldComponentDto.CropDtos.Count);

        fieldComponent.CropViewItems.Clear();
        fieldComponent.CropViewItems.Add(new CropViewItem());
        fieldComponent.CropViewItems.Add(new CropViewItem());
        fieldComponent.CropViewItems.Add(new CropViewItem());

        _fieldComponentService.ConvertCropViewItemsToDtoCollection(fieldComponent, fieldComponentDto);

        Assert.AreEqual(3, fieldComponentDto.CropDtos.Count);
    }

    [TestMethod]
    public void ConvertCropDtoCollectionToCropViewItemCollection()
    {
        var guid = Guid.NewGuid();

        var dto = new CropDto() {Guid = guid, AmountOfIrrigation = 200};

        _mockCropDtoFactory.Setup(x => x.CreateCropDto(It.IsAny<ICropDto>())).Returns(dto);

        var fieldComponent = new FieldSystemComponent() {CropViewItems = new ObservableCollection<CropViewItem>() {new CropViewItem() {Guid = guid}}};
        var fieldComponentDto = new FieldSystemComponentDto() {CropDtos = new ObservableCollection<ICropDto>(){dto}};

        _fieldComponentService.ConvertCropDtoCollectionToCropViewItemCollection(fieldComponent, fieldComponentDto);

        Assert.AreEqual(200, fieldComponent.CropViewItems[0].AmountOfIrrigation);
    }

    #endregion
}
