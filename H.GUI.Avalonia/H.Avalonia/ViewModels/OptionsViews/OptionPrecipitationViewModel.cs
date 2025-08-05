using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using DynamicData.Binding;
using H.Avalonia.ViewModels.Styles;
using H.Core.Enumerations;
using H.Core.Providers.Precipitation;
using H.Core.Services.StorageService;
using ImTools;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Prism.Events;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class OptionPrecipitationViewModel : ViewModelBase
    {
        #region Fields

        private ISeries[] _series;
        private Axis[] _xAxes;
        private BarChartStyles _barChartsStyles = new BarChartStyles();
        private PrecipitationDTO _data;

        #endregion

        #region Constructors
        public OptionPrecipitationViewModel()
        { 

        }
        public OptionPrecipitationViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IStorageService storageService) : base(regionManager, eventAggregator, storageService)
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

            this.InitializeData();     
            base.IsInitialized = true;
        }
        #endregion

        #region Properties

        public PrecipitationDTO Data
        {
            get => _data;
            set => SetProperty(ref  _data, value);
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

        public void InitializeData()
        {
            this.Data = new PrecipitationDTO(base.StorageService);
            this.Data.PropertyChanged -= this.DataOnPropertyChanged;
            this.Data.PropertyChanged += this.DataOnPropertyChanged;
            this.BuildChart();
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (!base.IsInitialized)
            {
                this.InitializeData();
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
                values.Add(Math.Round(this.Data.PrecipitationData.GetValueByMonth(month), 2));
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
            this.BuildChart();
        }

        #endregion

    }
}
