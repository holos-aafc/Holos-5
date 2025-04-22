using System.Collections.ObjectModel;
using H.Core;
using H.Core.Services.StorageService;
using Prism.Events;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class DefaultManureCompositionViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<DefaultManureCompositionDataViewModel> _defaultManureCompositionDataViewModels;
        private string _nitrogenFractionHeader;
        private string _carbonFractionHeader;
        private string _phosphorusFractionHeader;
        private string _moistureContentHeader;

        #endregion

        #region Constructors

        public DefaultManureCompositionViewModel(
            IRegionManager regionManager,
            IEventAggregator eventAggregator,
            IStorageService storageService) : base(regionManager, eventAggregator, storageService)
        {
            DefaultManureCompositionDataViewModels = new ObservableCollection<DefaultManureCompositionDataViewModel>();

            foreach(var dataClassInstance in base.ActiveFarm.DefaultManureCompositionData)
            {
                var dataClassViewModel = new DefaultManureCompositionDataViewModel(dataClassInstance);
                dataClassViewModel.SetSuppressValidationFlag(true);
                dataClassViewModel.MoistureContent = dataClassInstance.MoistureContent;
                dataClassViewModel.NitrogenFraction = dataClassInstance.NitrogenFraction;
                dataClassViewModel.CarbonFraction = dataClassInstance.CarbonFraction;
                dataClassViewModel.PhosphorusFraction = dataClassInstance.PhosphorusFraction;
                dataClassViewModel.CarbonToNitrogenRatio = dataClassInstance.CarbonToNitrogenRatio;
                dataClassViewModel.SetSuppressValidationFlag(false);
                DefaultManureCompositionDataViewModels.Add(dataClassViewModel);
            }
        }
         
        #endregion

        #region Properties

        public ObservableCollection<DefaultManureCompositionDataViewModel> DefaultManureCompositionDataViewModels
        {
            get => _defaultManureCompositionDataViewModels;
            set => SetProperty(ref _defaultManureCompositionDataViewModels, value);
        }

        public string NitrogenFractionHeader
        {
            get => _nitrogenFractionHeader;
            set => SetProperty(ref _nitrogenFractionHeader, value);
        }

        public string CarbonFractionHeader
        {
            get => _carbonFractionHeader;
            set => SetProperty(ref _carbonFractionHeader, value);
        }

        public string PhosphorusFractionHeader
        {
            get => _phosphorusFractionHeader;
            set => SetProperty(ref _phosphorusFractionHeader, value);
        }

        public string MoistureContentHeader
        {
            get => _moistureContentHeader;
            set => SetProperty(ref _moistureContentHeader, value);
        }

        #endregion

        #region Public Methods

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            SetStrings();
        }

        #endregion

        #region Private Methods

        private void SetStrings()
        {
            var displayUnits = StorageService.Storage.ApplicationData.DisplayUnitStrings;
            NitrogenFractionHeader = H.Core.Properties.Resources.LabelTotalNitrogen + " " + displayUnits.PercentageWetWeight;
            CarbonFractionHeader = H.Core.Properties.Resources.LabelTotalCarbon + " " + displayUnits.PercentageWetWeight;
            PhosphorusFractionHeader = H.Core.Properties.Resources.LabelTotalPhosphorus + " " + displayUnits.PercentageWetWeight;
            MoistureContentHeader = H.Core.Properties.Resources.LabelMoistureContent + " " + displayUnits.PercentageString;
        }

        #endregion
    }
}
