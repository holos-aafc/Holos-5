using H.CLI.Infrastructure;
using H.Core.Calculators.Carbon;
using H.Core.Services.LandManagement;
using Prism.Ioc;

namespace H.CLI.Test.Infrastructure
{
    /// <summary>
    /// Validates the CLI composition root: the container it builds (CoreModule + host infra)
    /// resolves the calculation graph the CLI uses, so the CLI no longer hand-constructs it.
    /// </summary>
    [TestClass]
    public class CliCompositionRootTest
    {
        [TestMethod]
        public void Build_ResolvesFieldResultsService()
        {
            var container = CliCompositionRoot.Build();

            Assert.IsNotNull(container.Resolve<IFieldResultsService>());
        }

        [TestMethod]
        public void Build_ResolvesCarbonService()
        {
            var container = CliCompositionRoot.Build();

            Assert.IsNotNull(container.Resolve<ICarbonService>());
        }
    }
}
