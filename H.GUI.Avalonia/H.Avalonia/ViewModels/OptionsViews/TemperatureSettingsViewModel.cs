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
using H.Avalonia.ViewModels.Styles;
using Avalonia.Metadata;
using ImTools;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class TemperatureSettingsViewModel : ViewModelBase
    {
        #region Fields

        private ISeries[] _series;
        private Axis[] _xAxes;
        private BarChartStyles _barChartsStyles = new BarChartStyles();
        private TemperatureData _data = new TemperatureData();

        #endregion

        #region Constructors

        public TemperatureSettingsViewModel()
        {

        }

        public TemperatureSettingsViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IStorageService storageService) : base(regionManager, eventAggregator, storageService)
        {
            _series = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Fill = _barChartsStyles.SetColumnSeriesFill(), 
                    Values = new ObservableCollection<double>()
                }
            };

            _xAxes = new Axis[]
            {
                _barChartsStyles.SetAxisStyling(name: H.Core.Properties.Resources.Months, labels: Enum.GetNames(typeof(Months)))
            };

            this.InitializeBindingTemperatureData();
            base.IsInitialized = true;
        }

        #endregion

        #region Properties

        public TemperatureData Data
        {
            get => _data;
            set => SetProperty(ref _data, value);
        }
        public ISeries[] Series
        {
            get => _series;
            set => SetProperty(ref _series, value);
        }

        public Axis[] XAxes
        {
            get => _xAxes;
            set => SetProperty(ref _xAxes, value);
        }

        #endregion

        #region Public Methods

        private void InitializeBindingTemperatureData()
        {
            if (base.ActiveFarm.ClimateData.TemperatureData != null)
            {
                this.Data = base.ActiveFarm.ClimateData.TemperatureData; // sharing references here
                this.Data.PropertyChanged -= DataOnPropertyChanged;
                this.Data.PropertyChanged += DataOnPropertyChanged;
            }
            else
            {
                throw new ArgumentNullException(nameof(base.ActiveFarm.ClimateData.TemperatureData));
            }
            this.BuildChart();
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (!base.IsInitialized)
            {
                this.InitializeBindingTemperatureData();
                base.IsInitialized = true;
            }
        }

        #endregion

        #region Private Methods

        private void BuildChart()
        {
            var values = new ObservableCollection<double>();
            foreach (Months month in Enum.GetValues(typeof(Months)))
            {
                values.Add(Math.Round(this.Data.GetValueByMonth(month), 2));
            }

            if (base.IsInitialized && !this.Series.IsNullOrEmpty() && this.Series[0] is ColumnSeries<double> columnSeries)
            {
                columnSeries.Values = values;
            }

            else
            {
                this.Series = new ISeries[]
                {
                        new ColumnSeries<double>
                        {
                            Fill = _barChartsStyles.SetColumnSeriesFill(),
                            Values = values,
                        }
                };
            }
        }

        #endregion

        #region Event Handlers

        private void DataOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is TemperatureData)
            {
                this.BuildChart();
            }
        }

        #endregion
    }
}