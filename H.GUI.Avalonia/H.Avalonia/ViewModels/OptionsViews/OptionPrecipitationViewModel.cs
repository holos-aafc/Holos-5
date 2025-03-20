using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
using Prism.Regions;
using H.Core.Services.StorageService;
using H.Core.Enumerations;
using BruTile.Wmts.Generated;
using DynamicData.Kernel;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using System.ComponentModel;
using H.Core.Providers.Precipitation;
using Prism.Events;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class OptionPrecipitationViewModel : ViewModelBase
    {
        #region Fields

        private PrecipitationData _bindingPrecipitationData = new PrecipitationData();

        #endregion

        #region Constructors

        public OptionPrecipitationViewModel()
        { 

        }

        public OptionPrecipitationViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IStorageService storageService) : base(regionManager, eventAggregator, storageService)
        {
            GetData();
            BindingPrecipitationData.PropertyChanged -= Data_PropertyChanged;
            BindingPrecipitationData.PropertyChanged += Data_PropertyChanged;
        }


        #endregion

        #region Properties
        
        public PrecipitationData BindingPrecipitationData
        {
            get => _bindingPrecipitationData;
            set
            {
                if (_bindingPrecipitationData != value)
                {
                    SetProperty(ref _bindingPrecipitationData, value);
                }
            }
        }
       
        #endregion

        #region Public Methods

        public void GetData()
        {
            BindingPrecipitationData = ActiveFarm.ClimateData.PrecipitationData;
            // This format was what was causing the event handler in the code behind to not fire.
            /* BindingPrecipitationData.January = ActiveFarm.ClimateData.PrecipitationData.January;
            BindingPrecipitationData.February = ActiveFarm.ClimateData.PrecipitationData.February;
            BindingPrecipitationData.March = ActiveFarm.ClimateData.PrecipitationData.March;
            BindingPrecipitationData.April = ActiveFarm.ClimateData.PrecipitationData.April;
            ... */  
        }

        #endregion

        #region Event Handler

        private void Data_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is PrecipitationData)
            {
                ActiveFarm.ClimateData.PrecipitationData = BindingPrecipitationData;
            }
        }

        #endregion

    }
}
