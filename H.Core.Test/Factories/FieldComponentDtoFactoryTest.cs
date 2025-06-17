using H.Core.Factories;
using H.Core.Models.LandManagement.Fields;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.ObjectModel;
using H.Core.Calculators.UnitsOfMeasurement;

namespace H.Core.Test.Factories;

[TestClass]
public class FieldComponentDtoFactoryTest
{
    #region Fields

    private IFieldComponentDtoFactory _factory;

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
        var mockCropDtoFactory = new Mock<ICropDtoFactory>();
        var mockUnitsOfMeasurementCalculator = new Mock<IUnitsOfMeasurementCalculator>();

        _factory = new FieldComponentDtoFactory(mockCropDtoFactory.Object, mockUnitsOfMeasurementCalculator.Object);
    }

    [TestCleanup]
    public void TestCleanup()
    {
    }

    #endregion

    #region Tests



    #endregion
}