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
                var defaultManureCompositionDataViewModel = new DefaultManureCompositionDataViewModel(dataClassInstance);
                DefaultManureCompositionDataViewModels.Add(defaultManureCompositionDataViewModel);
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
