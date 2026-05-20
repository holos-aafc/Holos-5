using H.Core;
using H.Core.Calculators.Carbon;
using H.Core.Services.Initialization;
using H.Core.Services.LandManagement;
using H.Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.DryIoc;
using Prism.Ioc;

namespace H.Core.Test
{
    /// <summary>
    /// Validates that <see cref="CoreModule"/> is a complete, self-contained registration of the
    /// calculation graph — i.e. the container can resolve the carbon/field services from
    /// <see cref="CoreModule"/> alone (plus the small set of host-provided infra: an
    /// <see cref="ILogger"/>, an <see cref="IMemoryCache"/> and an <see cref="ICacheService"/>).
    ///
    /// <para>
    /// This is the safety net for the DI consolidation: the GUI's container is registered through
    /// <c>ContainerRegistrationService</c> (which now delegates the calculation registrations to
    /// <see cref="CoreModule"/>), and the CLI will reuse <see cref="CoreModule"/> too. There is no
    /// automated test that builds the full GUI container, so this test guards the shared core.
    /// </para>
    /// </summary>
    [TestClass]
    public class CoreModuleResolutionTests
    {
        private static IContainerExtension BuildContainer()
        {
            var container = new DryIocContainerExtension();

            // Host-provided infra that CoreModule's registrations depend on but does not own
            // (both the GUI and the CLI supply these themselves).
            container.RegisterInstance<ILogger>(NullLogger.Instance);
            container.RegisterInstance<IMemoryCache>(new MemoryCache(new MemoryCacheOptions()));
            container.RegisterSingleton<ICacheService, InMemoryCacheService>();

            new CoreModule().RegisterTypes(container);
            container.FinalizeExtension();

            return container;
        }

        [TestMethod]
        public void CoreModule_ResolvesIcbmCarbonInputCalculator()
        {
            Assert.IsNotNull(BuildContainer().Resolve<IICBMCarbonInputCalculator>());
        }

        [TestMethod]
        public void CoreModule_ResolvesIpccTier2CarbonInputCalculator()
        {
            Assert.IsNotNull(BuildContainer().Resolve<IIPCCTier2CarbonInputCalculator>());
        }

        [TestMethod]
        public void CoreModule_ResolvesCarbonService()
        {
            Assert.IsNotNull(BuildContainer().Resolve<ICarbonService>());
        }

        [TestMethod]
        public void CoreModule_ResolvesCropInitializationService()
        {
            Assert.IsNotNull(BuildContainer().Resolve<ICropInitializationService>());
        }

        [TestMethod]
        public void CoreModule_ResolvesFieldResultsService()
        {
            // Exercises the deepest graph: FieldResultsService -> soil calculators ->
            // N2OEmissionFactorCalculator -> IClimateProvider -> ISlcClimateProvider, plus the
            // injected ICBM carbon-input calculator.
            Assert.IsNotNull(BuildContainer().Resolve<IFieldResultsService>());
        }

        [TestMethod]
        public void CoreModule_ResolvesFarmAnalysisService()
        {
            // The GUI's results entry point. Ctor: (IFieldResultsService, IAnimalService,
            // ShelterbeltCalculator) — all owned by CoreModule.
            Assert.IsNotNull(BuildContainer().Resolve<H.Core.Services.Analysis.IFarmAnalysisService>());
        }
    }
}
