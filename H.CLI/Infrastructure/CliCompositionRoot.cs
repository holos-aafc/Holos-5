using H.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Prism.DryIoc;
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
    /// of infrastructure it depends on. The CLI has no UI logging surface, so a
    /// <see cref="NullLogger"/> satisfies the <see cref="ILogger"/> dependency — diagnostic logging
    /// still flows through NLog at each call site via <c>LogManager.GetCurrentClassLogger()</c>.
    /// </para>
    ///
    /// <para><b>Not yet registered:</b> <c>IMemoryCache</c> / <c>ICacheService</c>. These are only
    /// needed to resolve <c>ICropInitializationService</c>, which the CLI does not resolve yet — it
    /// becomes relevant once command-line residue-input processing
    /// (<c>CarbonService.ProcessCommandLineItems</c>) is wired up. At that point add a
    /// <c>Microsoft.Extensions.Caching.Memory</c> package reference to H.CLI and register
    /// <c>IMemoryCache</c> + <c>ICacheService → InMemoryCacheService</c> here.</para>
    /// </summary>
    public static class CliCompositionRoot
    {
        public static IContainerExtension Build()
        {
            var container = new DryIocContainerExtension();

            // Host-provided infrastructure that CoreModule's registrations depend on.
            container.RegisterInstance<ILogger>(NullLogger.Instance);

            // Shared calculation graph (carbon/soil/N calculators, CarbonService, FieldResultsService,
            // providers, etc.).
            new CoreModule().RegisterTypes(container);

            container.FinalizeExtension();

            return container;
        }
    }
}
