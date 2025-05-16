using H.Core.Enumerations;
using H.Core.Providers.AnaerobicDigestion;
using H.Core.Providers.Climate;
using H.Core.Providers.Temperature;
using H.Core.Services.StorageService;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.ComponentModel;
using Prism.Events;
using H.Core.Services;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class OptionTemperatureViewModel : ViewModelBase
    {
        #region Fields

        private IInitializationService _initializationService = new InitializationService();

        private TemperatureData _bindingTemperatureData = new TemperatureData();

        #endregion

        #region Constructors

        public OptionTemperatureViewModel()
        {

        }

        public OptionTemperatureViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IStorageService storageService) : base(regionManager, eventAggregator, storageService)
        {
            InitializeBindingTemperatureData();

            BindingTemperatureData.PropertyChanged -= BindingTemperatureOnPropertyChanged;
            BindingTemperatureData.PropertyChanged += BindingTemperatureOnPropertyChanged;
        }

        #endregion

        #region Properties

        public TemperatureData BindingTemperatureData
        {
            get => _bindingTemperatureData;
            set
            {
                 _bindingTemperatureData = value;
            }
        }

        #endregion

        #region Public Methods

        private void InitializeBindingTemperatureData()
        {
            if (ActiveFarm.ClimateData.TemperatureData != null)
            {
                BindingTemperatureData = ActiveFarm.ClimateData.TemperatureData;
            }
            else
            {
                // Call initializationSerice?
            }
        }

        #endregion

        #region Event Handlers

        private void BindingTemperatureOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is TemperatureData)
            {
                ActiveFarm.ClimateData.TemperatureData = BindingTemperatureData;
            }
        }

        #endregion
    }
}