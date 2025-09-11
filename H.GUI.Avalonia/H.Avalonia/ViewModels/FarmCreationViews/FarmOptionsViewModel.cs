using System;
using System.Windows.Input;
using H.Avalonia.Views.FarmCreationViews;
using Prism.Commands;
using Prism.Regions;

namespace H.Avalonia.ViewModels.FarmCreationViews
{
    public class FarmOptionsViewModel : ViewModelBase
    {
        #region Fields

        private readonly IRegionManager _regionManager;

        #endregion

        #region Constructors

        public FarmOptionsViewModel(IRegionManager regionManager) : base(regionManager)
        {
            _regionManager = regionManager ?? throw new ArgumentNullException(nameof(regionManager));
            NavigateToCreateNewFarmCommand = new DelegateCommand(OnNavigateToCreateNewFarm);
            NavigateToOpenExistingFarmCommand = new DelegateCommand(OnNavigateToOpenExistingFarm);
            NavigateToImportFarmCommand = new DelegateCommand(OnNavigateToImportFarm);
        }

        #endregion

        #region Properties

        public ICommand NavigateToCreateNewFarmCommand { get; }
        public ICommand NavigateToOpenExistingFarmCommand { get; }
        public ICommand NavigateToImportFarmCommand { get; }

        #endregion

        #region Methods

        private void OnNavigateToOpenExistingFarm()
        {
            _regionManager.RequestNavigate(UiRegions.ContentRegion, nameof(FarmOpenExistingView));
        }

        private void OnNavigateToCreateNewFarm()
        {
            _regionManager.RequestNavigate(UiRegions.ContentRegion, nameof(FarmCreationView));
        }

        private void OnNavigateToImportFarm()
        {
            _regionManager.RequestNavigate(UiRegions.ContentRegion, nameof(Views.FarmCreationViews.FarmImportFileView));
        }
        #endregion
    }
}