
﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using H.Core.Enumerations;
using H.Core.Models;
using H.Core.Services.StorageService;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class OptionSoilViewModel : ViewModelBase
    {
        #region Fields

        SoilDisplayViewModel _data;

        #endregion

        #region Constructors
        public OptionSoilViewModel() { }
        public OptionSoilViewModel(IStorageService storageService) : base(storageService)
        {
            var globalSettings = StorageService.Storage.ApplicationData.GlobalSettings;
            globalSettings.PropertyChanged -= ActiveFarmChanged;
            globalSettings.PropertyChanged += ActiveFarmChanged;
        }
        #endregion

        #region Properties
        public SoilDisplayViewModel Data //{ get; set; }
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

        private void ActiveFarmChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GlobalSettings.ActiveFarm))
            {
                IsInitialized = false;
            }
        }

        #endregion
    }
}
