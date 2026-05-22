using H.CLI.Factories;
using H.CLI.Handlers;
using H.CLI.Infrastructure;
using H.CLI.Processors;
using H.Core.Calculators.Carbon;
using H.Core.Services;
using H.Core.Services.Initialization;
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

        [TestMethod]
        public void Build_ResolvesCropInitializationService()
        {
            // Depends on the host-provided ILogger / IMemoryCache / ICacheService registered above.
            var container = CliCompositionRoot.Build();

            Assert.IsNotNull(container.Resolve<ICropInitializationService>());
        }

        [TestMethod]
        public void Build_ResolvesFarmResultsService()
        {
            // FarmResultsService pulls in IEventAggregator + IADCalculator + IManureService +
            // IAnimalService + IFieldResultsService — all from the one container.
            var container = CliCompositionRoot.Build();

            Assert.IsNotNull(container.Resolve<IFarmResultsService>());
        }

        [TestMethod]
        public void Build_ResolvesCliProcessorsAndHandlers()
        {
            // The CLI orchestrators are now resolved from the container rather than hand-constructing
            // their own calculator stacks. Resolving the top of the chain proves the whole graph wires.
            var container = CliCompositionRoot.Build();

            Assert.IsNotNull(container.Resolve<FieldProcessor>());
            Assert.IsNotNull(container.Resolve<ComponentProcessorFactory>());
            Assert.IsNotNull(container.Resolve<ProcessorHandler>());
            Assert.IsNotNull(container.Resolve<ExportedFarmsHandler>());
        }
    }
}
