using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H.Avalonia.ViewModels.Styles;
using H.Core.Enumerations;
using H.Core.Providers.Evapotranspiration;
using H.Core.Services.StorageService;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Prism.Events;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class OptionEvapotranspirationViewModel : ViewModelBase
    {
        #region Fields

        private EvapotranspirationData _data = new EvapotranspirationData();
        private ISeries[] _series;
        private Axis[] _xAxes;
        private BarChartStyles _barChartsStyles = new BarChartStyles();

        #endregion

        #region Constructors

        public OptionEvapotranspirationViewModel()
        {
        }
        public OptionEvapotranspirationViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IStorageService storageService) : base(regionManager, eventAggregator, storageService)
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

            this.InitializeBindingEvapotranspirationData();
            base.IsInitialized = true;
        }

        #endregion

        #region Properties

        public EvapotranspirationData Data 
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

        public void InitializeBindingEvapotranspirationData()
        {
            if (base.ActiveFarm.ClimateData.EvapotranspirationData != null)
            {
                this.Data = base.ActiveFarm.ClimateData.EvapotranspirationData;
                this.Data.PropertyChanged -= DataOnPropertyChanged;
                this.Data.PropertyChanged += DataOnPropertyChanged;
            }
            else
            {
                throw new ArgumentNullException(nameof(base.ActiveFarm.ClimateData.EvapotranspirationData));
            }
            this.BuildChart();
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (!base.IsInitialized)
            {
                this.InitializeBindingEvapotranspirationData();
                base.IsInitialized = true;
            }
        }

        #endregion

        #region Private Methods

        private void BuildChart()
        {
            var values = new ObservableCollection<double> { };
            foreach (Months month in Enum.GetValues(typeof(Months)))
            {
                {
                    values.Add(Math.Round(this.Data.GetValueByMonth(month), 2));
                };
            }

            var columnSeries = new ColumnSeries<double>
            {
                Fill = _barChartsStyles.SetColumnSeriesFill(),
                Values = values,
            };

            this.Series = new ISeries[] { columnSeries };
        }

        #endregion

        #region Event Handlers

        private void DataOnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is EvapotranspirationData)
            {
                this.BuildChart();
            }
        }

        #endregion
    }
}
