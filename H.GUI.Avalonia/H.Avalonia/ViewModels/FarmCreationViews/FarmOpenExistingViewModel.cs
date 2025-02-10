using H.Avalonia.Views.FarmCreationViews;
using Prism.Commands;
using Prism.Regions;
using System.Windows.Input;
using H.Core.Services.StorageService;

namespace H.Avalonia.ViewModels
{
    public class FarmOpenExistingViewmodel : ViewModelBase
    {
        #region Fields
        private readonly IRegionManager _regionManager;
        #endregion

        #region Constructors
        public FarmOpenExistingViewmodel(IRegionManager regionManager, IStorageService storageService) : base(regionManager, storageService)
        {
            _regionManager = regionManager ?? throw new System.ArgumentNullException(nameof(regionManager));

            NavigateToPreviousPage = new DelegateCommand(OnNavigateToPreviousPage);
        }
        #endregion

        #region Properties

        public ICommand NavigateToPreviousPage { get; }

        #endregion

        #region Public Methods

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            var farms = base.StorageService.GetAllFarms();

            base.OnNavigatedTo(navigationContext);
        }

        #endregion

        #region Private Methods

        private void OnNavigateToPreviousPage()
        {
            _regionManager.RequestNavigate(UiRegions.ContentRegion, nameof(FarmOptionsView));
        }

        #endregion
    }
}
