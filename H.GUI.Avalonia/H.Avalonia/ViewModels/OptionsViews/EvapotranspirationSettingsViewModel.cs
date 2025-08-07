using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H.Avalonia.ViewModels.Styles;
using H.Core.Enumerations;
using H.Core.Providers.Evapotranspiration;
using H.Core.Providers.Temperature;
using H.Core.Services.StorageService;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Prism.Events;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class EvapotranspirationSettingsViewModel : ChartBaseViewModel<EvapotranspirationData>
    {
        #region Fields

        private EvapotranspirationData _data = new EvapotranspirationData();

        #endregion

        #region Constructors

        public EvapotranspirationSettingsViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IStorageService storageService) : base(regionManager, eventAggregator, storageService)
        {
            this.InitializeData();
            base.IsInitialized = true;
        }

        #endregion

        #region Properties

        public EvapotranspirationData Data 
        {
            get => _data;
            set => SetProperty(ref _data, value);
        }

        protected override EvapotranspirationData ChartValuesSource => Data;

        #endregion

        #region Public Methods

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (!base.IsInitialized)
            {
                this.InitializeData();
                base.IsInitialized = true;
            }
        }

        #endregion

        #region Protected Methods

        protected override void InitializeData()
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
            base.BuildChart();
        }

        #endregion

        #region Event Handlers

        private void DataOnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is EvapotranspirationData)
            {
                base.BuildChart();
            }
        }

        #endregion
    }
}
