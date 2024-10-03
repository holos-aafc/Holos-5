using H.Avalonia.Views;
using H.Core.Enumerations;
using H.Core.Services;
using H.Infrastructure;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace H.Avalonia.ViewModels.SupportingViews.MeasurementProvince
{
    public class MeasurementProvinceViewModel : BindableBase
    {
        #region Fields

        private MeasurementSystemType _selectedMeasurementSystem;
        private Province _selectedProvince;
        private readonly IRegionManager _regionManager;
        private readonly ICountrySettings _countrySettings;

        #endregion

        #region Constructors

        public MeasurementProvinceViewModel(IRegionManager regionManager, ICountrySettings countrySettings)
        {
            _regionManager = regionManager;
            _countrySettings = countrySettings;

            MeasurementSystemCollection = new ObservableCollection<MeasurementSystemType>(EnumHelper.GetValues<MeasurementSystemType>());
            ProvinceCollection = new ObservableCollection<Province>(EnumHelper.GetValues<Province>());

            // Set default selected province to the first one
            if (ProvinceCollection.Count > 0)
            {
                SelectedProvince = ProvinceCollection[0];
            }

            NavigateCommand = new DelegateCommand(OnNavigate);
        }

        #endregion

        #region Properties

        public ObservableCollection<MeasurementSystemType> MeasurementSystemCollection { get; set; }
        public ObservableCollection<Province> ProvinceCollection { get; set; }

        public MeasurementSystemType SelectedMeasurementSystem
        {
            get { return _selectedMeasurementSystem; }
            set { SetProperty(ref _selectedMeasurementSystem, value); }
        }

        public Province SelectedProvince
        {
            get { return _selectedProvince; }
            set { SetProperty(ref _selectedProvince, value); }
        }

        public ICommand NavigateCommand { get; }

        #endregion

        #region Methods

        private void OnNavigate()
        {
            // Navigate to next view
            if (_countrySettings.Version == CountryVersion.Canada)
            {
                _regionManager.RequestNavigate(UiRegions.ContentRegion, nameof(SoilDataView));
            }
            else
            {
                _regionManager.RequestNavigate(UiRegions.ContentRegion, nameof(ClimateDataView));
            }
        }

        #endregion
    }
}