using H.Core.Enumerations;
using H.Core.Properties;
using H.Core.Providers.Feed;
using H.Core.Services.DietService;
using H.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace H.Core.Test;

[TestClass]
public class DietFactoryTest
{
    #region Fields

    private IDietFactory _sut;

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
        var mockLogger = new Mock<ILogger>();
        var mockCacheService = new Mock<ICacheService>();
        var mockFeedIngredientProvider = new Mock<IFeedIngredientProvider>();

        _sut = new DietFactory(mockLogger.Object, mockCacheService.Object, mockFeedIngredientProvider.Object);
    }

    [TestCleanup]
    public void TestCleanup()
    {
    }

    #endregion

    #region Tests

    [TestMethod]
    public void CreateReturnsNonEmptyDiet()
    {
        var result = _sut.Create(DietType.LowEnergyAndProtein, AnimalType.BeefCow);

        Assert.AreEqual(Resources.LowEnergyProtein, result.Name);
    }

    [TestMethod]
    public void GetValidDietsReturnsNonZeroCount()
    {
        var result = _sut.GetValidDietKeys();

        Assert.IsTrue(result.Count > 0);
    }

    [TestMethod]
    public void Create_Parameterless_ThrowsNotImplementedException()
    {
        Assert.ThrowsException<NotImplementedException>(() => _sut.Create());
    }

    [TestMethod]
    public void IsValidDietType_ReturnsTrue_ForValidCombination()
    {
        var isValid = _sut.IsValidDietType(AnimalType.BeefCow, DietType.LowEnergyAndProtein);
        Assert.IsTrue(isValid);
    }

    [TestMethod]
    public void IsValidDietType_ReturnsFalse_ForInvalidCombination()
    {
        var isValid = _sut.IsValidDietType(AnimalType.Sheep, DietType.HighEnergyAndProtein);
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void Create_ReturnsUnknownDiet_ForInvalidCombination()
    {
        var result = _sut.Create(DietType.HighEnergyAndProtein, AnimalType.Sheep);
        Assert.AreEqual("Unknown diet", result.Name);
    }

    #endregion
}
