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

        public ISeries[] Series { get; set; } =
        {
            new ColumnSeries<double>
            {
                Values = new double[] {},
                Fill = new SolidColorPaint(SKColors.MediumAquamarine),
            }
        };

        public Axis[] XAxes { get; set; }
            = new Axis[]
            {
                new Axis
                {
                    NamePaint = new SolidColorPaint(SKColors.Black),

                    LabelsPaint = new SolidColorPaint(SKColors.Black),
                    TextSize = 14,
                    LabelsRotation = 20,
                    ShowSeparatorLines = true,

                    Labels = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"}
                }
            };


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
            CreateBarnTemperatureSeries();
        }

        #endregion

        #region Private Methods

        private void CreateBarnTemperatureSeries()
        {
            var values = new ObservableCollection<double> { };
            foreach (Months month in Enum.GetValues(typeof(Months)).Cast<Months>())
            {  
                {
                    values.Add(Math.Round(BindingTemperatureData.GetValueByMonth(month), 2));
                };
            }
            Series[0].Values = values;
        }

        #endregion

        #region Event Handlers

        private void BindingBarnTemperatureOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is TemperatureData)
            {
                CreateBarnTemperatureSeries();
                ActiveFarm.ClimateData.BarnTemperatureData = BindingTemperatureData;
            }
        }

        #endregion
    }
}