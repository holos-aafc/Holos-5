using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls.Notifications;
using H.Core.Services.StorageService;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews.FileMenuViews
{
    public class FileSaveViewModel : ViewModelBase
    {
        #region Fields

        #endregion

        #region Constructors

        public FileSaveViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IStorageService storageService) : base(regionManager, eventAggregator, storageService)
        {
            this.SaveCommand = new DelegateCommand(OnSaveExecute, CanExecuteSave);
        }

        #endregion

        #region Properties

        public ICommand SaveCommand { get; set; }

        public ICommand SaveAsCommand { get; set; }

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        #endregion

        #region Event Handlers

        private async void OnSaveExecute()
        {
            await base.StorageService.Storage.SaveAsync();
            NotificationManager?.Show(new Notification(
                title: "Save Success",
                message:"Holos saved successfully.",
                type: NotificationType.Success,
                expiration: TimeSpan.FromSeconds(10))
            );
        }

        private bool CanExecuteSave()
        {
            if (base.StorageService.Storage.SaveTask != null && base.StorageService.Storage.SaveTask.Status.Equals(TaskStatus.Running)) return false;

            return true;
        }

        #endregion
    }
}


