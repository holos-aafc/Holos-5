using System.Collections.ObjectModel;
using System.ComponentModel;
using H.Core.Calculators.UnitsOfMeasurement;
using H.Core.Services.StorageService;
using Prism.Events;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class DefaultBeddingCompositionViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<DefaultBeddingCompositionDataViewModel> _beddingCompositionDataViewModels;
        private IUnitsOfMeasurementCalculator _unitsCalculator;
        private string _nitrogenConcentrationHeader;
        private string _phosphorusConcentrationHeader;
        private string _carbonConcentrationHeader;

        #endregion

        #region Constructors

        public DefaultBeddingCompositionViewModel(
            IRegionManager regionManager,
            IEventAggregator eventAggregator,
            IStorageService storageService, 
            IUnitsOfMeasurementCalculator unitsCalculator) : base(regionManager, eventAggregator, storageService)
        {
            _unitsCalculator = unitsCalculator;
            _unitsCalculator.PropertyChanged -= UnitsOfMeasurementChangeListener;
            _unitsCalculator.PropertyChanged += UnitsOfMeasurementChangeListener;

            BeddingCompositionDataViewModels = new ObservableCollection<DefaultBeddingCompositionDataViewModel>();

            foreach (var dataClassInstance in base.ActiveFarm.DefaultsCompositionOfBeddingMaterials)
            {
                var dataClassViewModel = new DefaultBeddingCompositionDataViewModel(dataClassInstance, _unitsCalculator);
                dataClassViewModel.SetInitializationFlag(true);
                dataClassViewModel.TotalNitrogenKilogramsDryMatter = dataClassInstance.TotalNitrogenKilogramsDryMatter;
                dataClassViewModel.TotalPhosphorusKilogramsDryMatter = dataClassInstance.TotalPhosphorusKilogramsDryMatter;
                dataClassViewModel.TotalCarbonKilogramsDryMatter = dataClassInstance.TotalCarbonKilogramsDryMatter;
                dataClassViewModel.CarbonToNitrogenRatio = dataClassInstance.CarbonToNitrogenRatio;
                dataClassViewModel.SetInitializationFlag(false);
                BeddingCompositionDataViewModels.Add(dataClassViewModel);
            }
        }

        #endregion

        #region Properties

        public ObservableCollection<DefaultBeddingCompositionDataViewModel> BeddingCompositionDataViewModels
        {
            get => _beddingCompositionDataViewModels;
            set => SetProperty(ref _beddingCompositionDataViewModels, value);
        }

        public string NitrogenConcentrationHeader
        {
            get => _nitrogenConcentrationHeader;
            set => SetProperty(ref _nitrogenConcentrationHeader, value);
        }

        public string PhosphorusConcentrationHeader
        {
            get => _phosphorusConcentrationHeader;
            set => SetProperty(ref _phosphorusConcentrationHeader, value);
        }

        public string CarbonConcentrationHeader
        {
            get => _carbonConcentrationHeader;
            set => SetProperty(ref _carbonConcentrationHeader, value);
        }

        #endregion

        #region Public Methods

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            SetStrings(); 
        }

        #endregion

        #region Private Methods

        private void UnitsOfMeasurementChangeListener(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IUnitsOfMeasurementCalculator.IsMetric))
            {
                foreach (var viewModel in  BeddingCompositionDataViewModels)
                {
                    viewModel.UpdateUnitsOfMeasurementDependentProperties();
                }
            }
        }

        private void SetStrings()
        {
            var displayUnits = StorageService.Storage.ApplicationData.DisplayUnitStrings;
            NitrogenConcentrationHeader = H.Core.Properties.Resources.LabelTotalNitrogen + " " + displayUnits.KilogramsNitrogenPerKilogramDryMatter;
            PhosphorusConcentrationHeader = H.Core.Properties.Resources.LabelTotalPhosphorus + " " + displayUnits.KilogramsPhosphorusPerKilogramDryMatter;
            CarbonConcentrationHeader = H.Core.Properties.Resources.LabelTotalCarbon + " " + displayUnits.KilogramsCarbonPerKilogramDryMatter;
        }

        #endregion
    }
}
