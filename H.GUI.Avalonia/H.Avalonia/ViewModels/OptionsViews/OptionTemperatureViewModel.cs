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

        private Province _province = new Province();

        #endregion

        #region Constructors

        public OptionTemperatureViewModel()
        {

        }

        public OptionTemperatureViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IStorageService storageService) : base(regionManager, eventAggregator, storageService)
        {
            // Active farm currently will not have a selected province. Temp set to Alberta for testing purposes.
            _province = Province.Alberta;

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
                if (_bindingTemperatureData != value)
                {
                    _bindingTemperatureData = value;
                }
            }
        }

        #endregion

        #region Public Methods

        public void InitializeBindingTemperatureData()
        {
            if (ActiveFarm.ClimateData.TemperatureData != null)
            {
                BindingTemperatureData = ActiveFarm.ClimateData.TemperatureData;
            }
            else
            {
                var values = new TemperatureData();
                foreach (Months month in Enum.GetValues(typeof(Months)))
                {
                    {
                        values.GetValueByMonth(month);
                    };
                }
                BindingTemperatureData = values;
                BindingTemperatureData.IsInitialized = true;
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