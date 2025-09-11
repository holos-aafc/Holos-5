using H.Core.Enumerations;
using H.Core.Services.DietService;

namespace H.Core.Test.Services.DietService;

[TestClass]
public class DietServiceTest
{
    #region Fields

    private IDietService _sut;

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
        _sut = new DefaultDietService();
    }

    [TestCleanup]
    public void TestCleanup()
    {
    }

    #endregion

    #region Tests

    [TestMethod]
    public void GetValidAnimalDietTypesReturnsEmptyList()
    {
        var result = _sut.GetValidAnimalDietTypes(AnimalType.Horses);

        Assert.IsFalse(result.Any());
    }

    #endregion
}
