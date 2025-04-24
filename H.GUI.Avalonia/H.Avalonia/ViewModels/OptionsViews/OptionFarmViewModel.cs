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
        public OptionFarmViewModel(IStorageService storageService) : base(storageService)
        {
            Data = new FarmDisplayViewModel(storageService);

            _measurementSystemTypes = new ObservableCollection<MeasurementSystemType>() { MeasurementSystemType.Metric, MeasurementSystemType.Imperial };
            _selectedMeasurementType = StorageService.GetActiveFarm().MeasurementSystemType;
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
