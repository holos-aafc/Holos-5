using H.Core.Models;
using System.ComponentModel;
using H.Core.Services.StorageService;
using Prism.Regions;
using H.Avalonia.ViewModels.OptionsViews.DataTransferObjects;
using Prism.Events;
using Avalonia.Controls.Notifications;
using H.Core.Enumerations;
using System;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class SoilN2OBreakdownSettingsViewModel : ViewModelBase
    {
        #region Fields

        private SoilN2OBreakdownSettingsDTO _data;

        #endregion

        #region Constructors
        public SoilN2OBreakdownSettingsViewModel(IStorageService storageService) : base(storageService)
        {
            
        }
        #endregion

        #region Properties
        public SoilN2OBreakdownSettingsDTO Data
        {
            get => _data;
            set => SetProperty(ref _data, value);
        } 
        #endregion

        #region Public Methods

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (!IsInitialized)
            {
                Data = new SoilN2OBreakdownSettingsDTO(StorageService);
                Data.PropertyChanged += ValidateTotalLessThan100;
                IsInitialized = true;
            }
        }

        #endregion

        #region Event Handlers

        private void ValidateTotalLessThan100(object sender, PropertyChangedEventArgs e)
        {
            double total = 0;
            string errorTitle = "";

            foreach (Months month in Enum.GetValues(typeof(Months)))
            {
                total += this.Data.MonthlyValues.GetValueByMonth(month);
            }

            if (total == 100.00)
            {
                return;
            }
            else if (total < 100)
            {
                errorTitle = "N2O percentage total equals less than 100%.";

            }
            else if (total > 100)
            {
                errorTitle = "N2O percentage total equals more than 100%.";
            }
            NotificationManager?.Show(new Notification(
                title: errorTitle,
                message: $"Values currently add up to {total}%.",
                type: NotificationType.Warning,
                expiration: TimeSpan.FromSeconds(10)));
        }

        #endregion
    }
}
