using System.Collections.ObjectModel;
using H.Core.Services.StorageService;
using Prism.Events;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class DefaultManureCompositionViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<DefaultManureCompositionDataViewModel> _defaultManureCompositionDataViewModels;

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

        #endregion
    }
}
