
﻿using System.Collections.ObjectModel;
using H.Core.Enumerations;
using H.Core.Services.StorageService;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class OptionSoilViewModel : ViewModelBase
    {
        #region Constructors
        public OptionSoilViewModel() { }
        public OptionSoilViewModel(IStorageService storageService) : base(storageService)
        {
            Data = new SoilDisplayViewModel(storageService);
        }
        #endregion

        #region Properties
        public SoilDisplayViewModel Data { get; set; }
        #endregion

    }
}
