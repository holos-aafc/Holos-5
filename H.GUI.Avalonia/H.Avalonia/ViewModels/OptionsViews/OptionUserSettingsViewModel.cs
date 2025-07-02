
using System;
using System.ComponentModel;
using H.Core.Models;
using H.Core.Services.StorageService;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class OptionUserSettingsViewModel : ViewModelBase
    {
        #region Fields

        private UserSettingsDisplayViewModel _data;

        #endregion

        #region Constructors

        public OptionUserSettingsViewModel() { }

        public OptionUserSettingsViewModel(IStorageService storageService) : base(storageService)
        {

        }

        #endregion

        #region Properties

        public UserSettingsDisplayViewModel Data
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
                Data = new UserSettingsDisplayViewModel(base.StorageService);
                base.IsInitialized = true;
            }
        }

        #endregion

        #region Event Handlers

        #endregion
    }
}
