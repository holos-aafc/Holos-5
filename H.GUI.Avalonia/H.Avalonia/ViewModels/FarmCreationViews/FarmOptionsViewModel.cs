using Prism.Commands;
using Prism.Regions;
using System.Windows.Input;
using System;
using H.Avalonia.Views.FarmCreationViews;

namespace H.Avalonia.ViewModels
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
        }

        #endregion

        #region Properties

        public ICommand NavigateToCreateNewFarmCommand { get; }

        #endregion

        #region Methods

        private void OnNavigateToCreateNewFarm()
        {
            _regionManager.RequestNavigate(UiRegions.ContentRegion, nameof(FarmCreationView));
        }

        #endregion
    }
}