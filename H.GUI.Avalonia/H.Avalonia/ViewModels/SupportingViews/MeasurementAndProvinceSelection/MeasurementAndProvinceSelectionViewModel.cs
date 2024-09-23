#region Imports

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using H.Avalonia.ViewModels;
using H.Core;
using H.Core.Calculators.UnitsOfMeasurement;
using H.Core.Enumerations;
using H.Core.Properties;
using H.Events;
using H.Views.SupportingViews.Map;
using H.Views.SupportingViews.ProvinceSelection;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

#endregion

namespace H.Views.SupportingViews.MeasurementAndProvinceSelection
{
    /// <summary>
    /// ViewModel that handles both Units of Measurement and Province Selection.
    /// </summary>
    public class MeasurementAndProvinceSelectionViewModel : ViewModelBase, INavigationAware
    {
        #region Properties

        public ObservableCollection<MeasurementSystemType> MeasurementSystems { get; set; } = new ObservableCollection<MeasurementSystemType>()
        {
            MeasurementSystemType.Metric
        };

        public ObservableCollection<Province> Provinces { get; set; }

        #endregion

        #region Fields

        private readonly List<Province> _canadianProvinces = new List<Province>()
        {
            Province.Alberta, Province.BritishColumbia, Province.Manitoba, Province.NewBrunswick,
            Province.Newfoundland, Province.NorthwestTerritories, Province.NovaScotia, Province.Ontario,
            Province.Nunavut, Province.PrinceEdwardIsland, Province.Quebec, Province.Saskatchewan, Province.Yukon,
        };

        #endregion

        #region Constructors

        public MeasurementAndProvinceSelectionViewModel()
        {
        }

        public MeasurementAndProvinceSelectionViewModel(Storage storage,
                                                        IRegionManager regionManager,
                                                        IEventAggregator eventAggregator) : base(regionManager, eventAggregator, storage)
        {
            // Initialize Provinces (excluding Canadian provinces)
            this.Provinces = new ObservableCollection<Province>(Enum.GetValues(typeof(Province))
                                                                    .Cast<Province>()
                                                                    .Except(_canadianProvinces));

            // Initialize commands
            this.NextCommand = new DelegateCommand(this.OnNext);
        }

        #endregion

        #region Public Methods

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            // Publish event for Measurement Unit selection view
            EventAggregator.GetEvent<ViewChangedEvent>().Publish(typeof(UnitsOfMeasurementView));

            if (this.ActiveFarm.IsBasicMode)
            {
                this.OnNext();
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        #endregion

        #region Private Methods

        private void OnNext()
        {
            // Save measurement system settings
            Settings.Default.MeasurementSystem = this.Storage.ApplicationData.GlobalSettings.ActiveFarm.MeasurementSystemType;
            Settings.Default.Save();

            this.Storage.ApplicationData.GlobalSettings.ActiveFarm.MeasurementSystemSelected = true;
            this.Storage.ApplicationData.DisplayUnitStrings.SetStrings(this.Storage.ApplicationData.GlobalSettings.ActiveFarm.MeasurementSystemType);

            // Navigate to Province Selection view
            EventAggregator.GetEvent<ViewChangedEvent>().Publish(typeof(ProvinceView));
            base.RegionManager.RequestNavigate(Regions.ContentRegion, nameof(ProvinceView));
        }

        #endregion

        #region Event Handlers

        private void OnProvinceSelected()
        {
            // Navigate to Map View after selecting a province
            RegionManager.RequestNavigate(Regions.ContentRegion, nameof(MapView));
        }

        #endregion
    }
}
