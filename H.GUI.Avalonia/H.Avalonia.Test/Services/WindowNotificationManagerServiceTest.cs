using Avalonia;
using Avalonia.Controls;
using H.Avalonia.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Avalonia.Controls.Notifications;
using Avalonia.Headless;

#nullable disable

namespace H.Avalonia.Test.Services
{
    [TestClass]
    public class NotificationManagerServiceTest
    {
        private NotificationManagerService _service = null!;
        private Mock<ILogger> _mockLogger = null!;
        private ILogger _loggerMock = null!;
        private Window _window = null!;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            var headlessOptions = new AvaloniaHeadlessPlatformOptions();
            AppBuilder.Configure<App>().UseHeadless(headlessOptions).SetupWithoutStarting();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _mockLogger = new Mock<ILogger>();
            _loggerMock = _mockLogger.Object;
            _window = new Window();
        }

        [TestMethod]
        public void TestConstructorInitialization()
        {
            _service = new NotificationManagerService(_loggerMock);
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void TestConstructorThrowsExceptionOnNullParameter()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() => new NotificationManagerService(null));
        }

        [TestMethod]
        public void TestInitializeMethodForNullWindow()
        {
            _service = new NotificationManagerService(_loggerMock);
            Assert.ThrowsExactly<ArgumentNullException>(() => _service.Initialize(null));
        }

        [TestMethod]
        public void TestInitializationValidWindow()
        {
            _service = new NotificationManagerService(_loggerMock);
            _service.Initialize(_window);
            Assert.IsTrue(_service.IsInitialized);
            _mockLogger.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.AtMostOnce);
        }

        [TestMethod]
        public void TestReinitializationWindow()
        {
            _service = new NotificationManagerService(_loggerMock);
            _service.Initialize(_window);
            _service.Initialize(_window);
            _mockLogger.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.AtLeast(2));
        }

        [TestMethod]
        public void TestShowOneToast()
        {
            _service = new NotificationManagerService(_loggerMock);
            UsePersistentNotifications(_service);
            _service.Initialize(_window);
            _service.ShowToast("Test Title", "Test Message");
            Assert.AreEqual(1, _service.ActiveNotifications.Count);
        }

        [TestMethod]
        public void TestShowMoreToasts()
        {
            _service = new NotificationManagerService(_loggerMock);
            UsePersistentNotifications(_service);
            _service.Initialize(_window);
            _service.ShowToast("Test Title1", "Test Message1");
            _service.ShowToast("Test Title2", "Test Message2");
            _service.ShowToast("Test Title3", "Test Message3");
            _service.ShowToast("Test Title4", "Test Message4");
            _service.ShowToast("Test Title5", "Test Message5");

            Assert.AreEqual(5, _service.ActiveNotifications.Count);
        }

        /// <summary>
        /// Makes notifications effectively persistent for the duration of the test run.
        /// <para>
        /// <see cref="Avalonia.Controls.Notifications.WindowNotificationManager.Show"/> schedules an
        /// internal <c>Task.Delay(expiration)</c> that then calls <c>NotificationCard.Close()</c>.
        /// Under the headless <c>SetupWithoutStarting()</c> harness there is no running dispatcher, so
        /// that continuation runs on a thread-pool thread and <c>Close()</c> throws
        /// "Call from invalid thread". With the production 5–10 s durations the callback can fire
        /// while a later, slower test keeps the process alive — crashing the whole test host. Using a
        /// duration longer than any test run guarantees the callback never fires before the process
        /// exits, while leaving the assertions (which read the service's own collection) unaffected.
        /// </para>
        /// </summary>
        private static void UsePersistentNotifications(NotificationManagerService service)
        {
            var persistent = TimeSpan.FromHours(1);
            service.SuccessDuration = persistent;
            service.InformationDuration = persistent;
            service.WarningDuration = persistent;
            service.ErrorDuration = persistent;
        }

        // Adds unnecessary time to execution of tests, so ignored by default
        [TestMethod]
        [Ignore]
        public void TestMessageTimer()
        {
            _service = new NotificationManagerService(_loggerMock);
            _service.Initialize(_window);
            _service.ShowToast("Test Title1", "Test Message1", NotificationType.Success);
            _service.ShowToast("Test Title2", "Test Message2", NotificationType.Error);

            Thread.Sleep(TimeSpan.FromSeconds(6));

            Assert.AreEqual(1, _service.ActiveNotifications.Count);
        }
    }
}
