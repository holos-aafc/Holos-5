using H.Core.Enumerations;
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

        _sut = new DietFactory(mockLogger.Object, mockCacheService.Object);
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

        Assert.AreEqual("Holos Diet", result.Name);
    }

    [TestMethod]
    public void GetValidDietsReturnsNonZeroCount()
    {
        var result = _sut.GetValidDiets();

        Assert.IsTrue(result.Count > 0);
    }

    #endregion
}
