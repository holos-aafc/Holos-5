using H.CLI.Infrastructure;
using H.CLI.Processors;
using Prism.Ioc;

namespace H.CLI.Test.Processors
{
    [TestClass]
    public class ProccessorHandlerTest
    {
        [TestMethod]
        public void TestSetProcessor()
        {
            var processorHandler = CliCompositionRoot.Build().Resolve<ProcessorHandler>();
            processorHandler.SetProccessor(new ShelterbeltProcessor());
            Assert.IsInstanceOfType(processorHandler._processor, typeof(ShelterbeltProcessor));
        }
    }
}
