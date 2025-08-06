using H.Core.Enumerations;
using System.Collections.ObjectModel;
using H.Core.Services.StorageService;
using Prism.Regions;
using H.Core.Models;
using System.ComponentModel;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class FarmSettingsViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<MeasurementSystemType> _measurementSystemTypes;
        private MeasurementSystemType _selectedMeasurementType;
        private FarmDTO _data;

        #endregion

        #region Constructors
        public FarmSettingsViewModel() { }
        public FarmSettingsViewModel(IStorageService storageService) : base(storageService)
        {
            _measurementSystemTypes = new ObservableCollection<MeasurementSystemType>() { MeasurementSystemType.Metric, MeasurementSystemType.Imperial };
        }

        #endregion

        #region Properties

        public FarmDTO Data
        {
            get => _data;
            set => SetProperty(ref _data, value);
        }

        public ObservableCollection<MeasurementSystemType> MeasurementSystemTypes
        {
            get => _measurementSystemTypes; 
        }

        public MeasurementSystemType SelectedMeasurementSystem
        {
            get => _selectedMeasurementType; 
            set
            {
                if (SetProperty(ref _selectedMeasurementType, value))
                {
                    if (IsInitialized && MeasurementSystemTypes.Contains(value)) 
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

        #region Public Methods

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (!IsInitialized)
            {
                Data = new FarmDTO(StorageService);
                SelectedMeasurementSystem = StorageService.GetActiveFarm().MeasurementSystemType;
                IsInitialized = true;
            }
        }

        #endregion

        #region Event Handlers

        #endregion
    }
}
