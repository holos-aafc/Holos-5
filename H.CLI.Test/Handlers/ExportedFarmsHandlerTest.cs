#region Imports

using H.CLI.Handlers;
using H.CLI.Infrastructure;
using Prism.Ioc;

#endregion

namespace H.CLI.Test.Handlers
{
    [TestClass]
    public class ExportedFarmsHandlerTest
    {
        #region Fields

        // Built once for the class; CliCompositionRoot loads the SmallAreaYieldProvider CSV.
        private static readonly IContainerExtension _container = CliCompositionRoot.Build();
        private ExportedFarmsHandler _handler = null!;

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
            _handler = _container.Resolve<ExportedFarmsHandler>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        #endregion

        #region Tests

        [TestMethod]
        [Ignore]
        public void PromptUserForLocationOfExportedFarms()
        {
            _handler.PromptUserForLocationOfExportedFarms("");
        }

        #endregion
    }
}
