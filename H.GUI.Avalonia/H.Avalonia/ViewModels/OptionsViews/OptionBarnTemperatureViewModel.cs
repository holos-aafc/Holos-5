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
    public class OptionBarnTemperatureViewModel : ViewModelBase
    {
        #region Fields
        // Can use InitializationService once implemented in V5 to set barn temperatures
        private IInitializationService _initializationService = new InitializationService();

        private TemperatureData _bindingTemperatureData = new TemperatureData();
        private IIndoorTemperatureProvider _indoorTemperatureProvider = new Table_63_Indoor_Temperature_Provider();

        private Province _province = new Province();

        #endregion

        #region Constructors

        public OptionBarnTemperatureViewModel()
        {

        }

        public OptionBarnTemperatureViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IStorageService storageService) : base(regionManager, eventAggregator, storageService)
        {
            // Active farm currently will not have a selected province. Temp set to Alberta for testing purposes.
            _province = Province.Alberta;

            InitializeBindingBarnTemperatureData();

            BindingTemperatureData.PropertyChanged -= BindingBarnTemperatureOnPropertyChanged;
            BindingTemperatureData.PropertyChanged += BindingBarnTemperatureOnPropertyChanged;
        }

        #endregion

        #region Properties

        public TemperatureData BindingTemperatureData
        {
            get => _bindingTemperatureData;
            set
            {
                if (_bindingTemperatureData != value)
                {
                    _bindingTemperatureData = value;
                }
            }
        }

        #endregion

        #region Public Methods

        public void InitializeBindingBarnTemperatureData()
        {
            if (ActiveFarm.ClimateData.BarnTemperatureData != null)
            {
                BindingTemperatureData = ActiveFarm.ClimateData.BarnTemperatureData;
            }
            else
            {
                BindingTemperatureData = _indoorTemperatureProvider.GetIndoorTemperature(_province);
                BindingTemperatureData.IsInitialized = true;
            }
        }

        #endregion

        #region Event Handlers

        private void BindingBarnTemperatureOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is TemperatureData)
            {
                ActiveFarm.ClimateData.BarnTemperatureData = BindingTemperatureData;
            }
        }

        #endregion
    }
}