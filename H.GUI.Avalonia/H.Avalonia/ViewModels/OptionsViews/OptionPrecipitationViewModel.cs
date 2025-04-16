using Prism.Regions;
using H.Core.Services.StorageService;
using H.Core.Providers.Precipitation;
using Prism.Events;
using DynamicData.Binding;
using H.Core.Enumerations;
using System;
using System.ComponentModel;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class OptionPrecipitationViewModel : ViewModelBase
    {

        #region Constructors
        public OptionPrecipitationViewModel()
        { 

        }
        public OptionPrecipitationViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IStorageService storageService) : base(regionManager, eventAggregator, storageService)
        {
            Data = new PrecipitationDisplayViewModel(storageService);           
        }
        #endregion

        #region Properties
        public PrecipitationDisplayViewModel Data { get; set; }
        #endregion

    }
}
