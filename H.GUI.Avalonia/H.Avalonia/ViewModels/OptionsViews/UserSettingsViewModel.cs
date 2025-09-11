
using System;
using System.ComponentModel;
using H.Avalonia.ViewModels.OptionsViews.DataTransferObjects;
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
            this.Initialize();
            base.IsInitialized = true;
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

        public void Initialize()
        {
            Data = new UserSettingsDTO(base.StorageService);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (!base.IsInitialized)
            {
                this.Initialize();
                base.IsInitialized = true;
            }
        }

        #endregion

        #region Event Handlers

        #endregion
    }
}
