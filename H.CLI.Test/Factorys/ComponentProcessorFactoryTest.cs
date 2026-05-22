using H.CLI.Factories;
using H.CLI.Infrastructure;
using H.CLI.Processors;
using H.Core.Models.LandManagement.Shelterbelt;
using Prism.Ioc;

namespace H.CLI.Test.Factorys
{
    [TestClass]
    public class ComponentProcessorFactoryTest
    {
        [TestMethod]
        public void TestComponentProcessorFactory()
        {
            // Resolve from the CLI composition root so the factory gets its processors from the
            // shared calculator graph (this also exercises the CLI DI registrations).
            var componentProcessorFactory = CliCompositionRoot.Build().Resolve<ComponentProcessorFactory>();
            var result = componentProcessorFactory.GetComponentProcessor(new ShelterbeltComponent().GetType());
            Assert.IsInstanceOfType(result, typeof(ShelterbeltProcessor));

        }

    }
}
