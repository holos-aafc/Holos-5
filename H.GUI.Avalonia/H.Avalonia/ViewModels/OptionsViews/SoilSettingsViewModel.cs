
﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using H.Core.Enumerations;
using H.Core.Models;
using H.Core.Services.StorageService;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class SoilSettingsViewModel : ViewModelBase
    {
        #region Fields

        SoilDisplayViewModel _data;

        #endregion

        #region Constructors
        public SoilSettingsViewModel() { }
        public SoilSettingsViewModel(IStorageService storageService) : base(storageService)
        {

        }
        #endregion

        #region Properties
        public SoilDisplayViewModel Data 
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
                Data = new SoilDisplayViewModel(StorageService);
                IsInitialized = true;
            }
        }

        #endregion

        #region Event Handlers

        #endregion
    }
}
