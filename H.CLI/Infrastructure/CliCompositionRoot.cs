using H.CLI.Factories;
using H.CLI.Handlers;
using H.CLI.Processors;
using H.Core;
using H.Core.Calculators.Infrastructure;
using H.Core.Services;
using H.Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Prism.DryIoc;
using Prism.Events;
using Prism.Ioc;

namespace H.CLI.Infrastructure
{
    /// <summary>
    /// Composition root for the command-line app. Builds a Prism.DryIoc container that reuses
    /// <see cref="CoreModule"/> — the same calculation-registration module the Avalonia GUI uses —
    /// so the CLI resolves the identical calculator/service graph instead of hand-constructing it.
    ///
    /// <para>
    /// <see cref="CoreModule"/> owns the calculation registrations; the host supplies the small set
    /// of infrastructure it depends on: an <see cref="ILogger"/>, an <see cref="IMemoryCache"/> and
    /// an <see cref="ICacheService"/>. The CLI has no UI logging surface, so a
    /// <see cref="NullLogger"/> satisfies the <see cref="ILogger"/> dependency — diagnostic logging
    /// still flows through NLog at each call site via <c>LogManager.GetCurrentClassLogger()</c>.
    /// </para>
    /// </summary>
    public static class CliCompositionRoot
    {
        public static IContainerExtension Build()
        {
            var container = new DryIocContainerExtension();

            // Host-provided infrastructure that CoreModule's registrations depend on.
            container.RegisterInstance<ILogger>(NullLogger.Instance);
            container.RegisterInstance<IMemoryCache>(new MemoryCache(new MemoryCacheOptions()));
            container.RegisterSingleton<ICacheService, InMemoryCacheService>();

            // Shared calculation graph (carbon/soil/N calculators, CarbonService, FieldResultsService,
            // CropInitializationService, providers, etc.).
            new CoreModule().RegisterTypes(container);

            // Farm-level emission aggregation used by the CLI's report writers. CoreModule already
            // supplies IFieldResultsService / IAnimalService / IManureService; the rest are host
            // services the CLI's FarmResultsService + report path depend on.
            container.RegisterSingleton<IEventAggregator, EventAggregator>();
            container.RegisterSingleton<IADCalculator, ADCalculator>();
            container.RegisterSingleton<IFarmResultsService, FarmResultsService>();
            container.RegisterSingleton<ITimePeriodHelper, TimePeriodHelper>();

            // CLI component-processing orchestrators. Registering them here means they share the one
            // calculator graph above instead of each hand-constructing their own.
            container.Register<FieldProcessor>();
            container.Register<ShelterbeltProcessor>();
            container.Register<ComponentProcessorFactory>();
            container.Register<ProcessorHandler>();
            container.Register<ExportedFarmsHandler>();

            container.FinalizeExtension();

            return container;
        }
    }
}
