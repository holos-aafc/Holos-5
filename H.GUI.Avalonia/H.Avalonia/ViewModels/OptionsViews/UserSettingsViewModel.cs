
using System;
using System.ComponentModel;
using H.Core.Models;
using H.Core.Services.StorageService;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class UserSettingsViewModel : ViewModelBase
    {
        #region Fields

        private UserSettingsDTO _data;

        #endregion

        #region Constructors

        public UserSettingsViewModel() { }

        public UserSettingsViewModel(IStorageService storageService) : base(storageService)
        {

        }

        #endregion

        #region Properties

        public UserSettingsDTO Data
        {
            get => _data;
            set => SetProperty(ref _data, value);
        }

        #endregion

        #region Public Methods 

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (!base.IsInitialized)
            {
                Data = new UserSettingsDTO(base.StorageService);
                base.IsInitialized = true;
            }
        }

        #endregion

        #region Event Handlers

        #endregion
    }
}
