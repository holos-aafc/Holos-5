using H.Core.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        _factory = new FieldComponentDtoFactory();
    }

    [TestCleanup]
    public void TestCleanup()
    {
    }

    #endregion
}