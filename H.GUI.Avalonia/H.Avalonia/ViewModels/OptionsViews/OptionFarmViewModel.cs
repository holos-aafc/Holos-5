using H.Core.Enumerations;
using System.Collections.ObjectModel;
using H.Core.Services.StorageService;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class OptionFarmViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<MeasurementSystemType> _measurementSystemTypes;
        private MeasurementSystemType _selectedMeasurementType;

        #endregion

        #region Constructors
        public OptionFarmViewModel() { }

        public OptionFarmViewModel(IRegionManager regionManager, IStorageService storageService) : base(regionManager, storageService)
        {
            var activeFarm = StorageService.GetActiveFarm();
            Data = new FarmDisplayViewModel(storageService);
            Data.FarmName = activeFarm.Name;
            Data.FarmComments = activeFarm.Comments;
            Data.Coordinates = $"{activeFarm.Latitude}, {activeFarm.Longitude}";
            Data.GrowingSeasonPrecipitation = activeFarm.ClimateData.PrecipitationData.GrowingSeasonPrecipitation;
            Data.GrowingSeasonEvapotranspiration = activeFarm.ClimateData.EvapotranspirationData.GrowingSeasonEvapotranspiration;

            _measurementSystemTypes = new ObservableCollection<MeasurementSystemType>() { MeasurementSystemType.Metric, MeasurementSystemType.Imperial };
            _selectedMeasurementType = activeFarm.MeasurementSystemType;
        }

        #endregion

        #region Properties

        public FarmDisplayViewModel Data { get; set; }

        public ObservableCollection<MeasurementSystemType> MeasurementSystemTypes
        {
            get { return _measurementSystemTypes; }
        }

        public MeasurementSystemType SelectedMeasurementSystem
        {
            get { return _selectedMeasurementType; }
            set
            {
                if (SetProperty(ref _selectedMeasurementType, value))
                {
                    if (value == MeasurementSystemType.Metric || value == MeasurementSystemType.Imperial)
                    {
                        var activeFarm = StorageService.GetActiveFarm();
                        activeFarm.MeasurementSystemType = value;
                        activeFarm.MeasurementSystemSelected = true;
                        StorageService.Storage.ApplicationData.DisplayUnitStrings.SetStrings(StorageService.GetActiveFarm().MeasurementSystemType);
                    }
                }
            }
        }

        #endregion
    }
}
