using H.Avalonia.Services;
using System;

namespace H.Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private readonly INotificationManagerService _notificationManagerService;

        #endregion

        #region Properties

        public INotificationManagerService NotificationManagerService
        {
            get => _notificationManagerService;
        }

        #endregion

        #region Constructor

        public MainWindowViewModel(INotificationManagerService notificationManagerService) : base()
        {
            _notificationManagerService = notificationManagerService ?? throw new ArgumentNullException(nameof(notificationManagerService));
        }

        #endregion
    }
}
