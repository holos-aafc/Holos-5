using H.Avalonia.Infrastructure.DependencyInjection;
using H.Avalonia.Services;
using H.Core.Services.LandManagement;
using H.Core.Services.StorageService;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Prism.DryIoc;
using Prism.Events;
using Prism.Ioc;

namespace H.Avalonia.Test.Infrastructure
{
    /// <summary>
    /// Builds the full GUI dependency-injection container through
    /// <see cref="ContainerRegistrationService.RegisterAllTypes"/> and verifies the top-level
    /// services resolve. This is the automated companion to the manual GUI smoke test: it guards
    /// the GUI-only registrations (services, mappers, factories, transfer services, dialogs) that
    /// sit on top of the shared <c>CoreModule</c> graph.
    /// </summary>
    [TestClass]
    public class FullContainerResolutionTests
    {
        // Built once for the class: RegisterAllTypes loads storage and pre-warms the heavy
        // SmallAreaYieldProvider CSV, so sharing one container keeps the suite fast.
        private static IContainerExtension _container = null!;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            var container = new DryIocContainerExtension();

            // Prerequisites the application normally supplies before RegisterTypes runs:
            // App.SetUpLogging registers ILogger; PrismApplication registers IEventAggregator.
            container.RegisterInstance<ILogger>(NullLogger.Instance);
            container.RegisterSingleton<IEventAggregator, EventAggregator>();

            var registrationService = new ContainerRegistrationService(container, NullLogger.Instance);
            registrationService.RegisterAllTypes(container);
            container.FinalizeExtension();

            _container = container;
        }

        [TestMethod]
        public void FullContainer_ResolvesStorageService()
        {
            Assert.IsNotNull(_container.Resolve<IStorageService>());
        }

        [TestMethod]
        public void FullContainer_ResolvesNotificationManagerService()
        {
            Assert.IsNotNull(_container.Resolve<INotificationManagerService>());
        }

        [TestMethod]
        public void FullContainer_ResolvesErrorHandlerService()
        {
            Assert.IsNotNull(_container.Resolve<IErrorHandlerService>());
        }

        [TestMethod]
        public void FullContainer_ResolvesFieldResultsService()
        {
            Assert.IsNotNull(_container.Resolve<IFieldResultsService>());
        }
    }
}
