using H.Avalonia.Views.FarmCreationViews;
using Prism.Commands;
using Prism.Regions;
using System.Windows.Input;
using H.Avalonia.Views.ComponentViews;
using H.Core.Models;
using H.Core.Services.StorageService;

namespace H.Avalonia.ViewModels
{
    public class FarmCreationViewModel : ViewModelBase
    {
        #region Fields

        private string _farmName;
        private string _farmComments;


        #endregion

        #region Constructors

        public FarmCreationViewModel()
        {
        }

        public FarmCreationViewModel(IRegionManager regionManager, IStorageService storageService) : base(regionManager, storageService)
        {
            NavigateToPreviousPage = new DelegateCommand(OnNavigateToPreviousPage);
        }

        #endregion

        #region Properties

        public ICommand NavigateToPreviousPage { get; }

        public string FarmName
        {
            get => _farmName;
            set => SetProperty(ref _farmName, value);
        }

        public string FarmComments
        {
            get => _farmComments;
            set => SetProperty(ref  _farmComments, value);
        }

        #endregion

        #region Public Methods

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
        }

        public void OnCreateNewFarmExecute()
        {
            if (string.IsNullOrWhiteSpace(this.FarmName) == false)
            {
                var farm = new Farm()
                {
                    Name = this.FarmName,
                    Comments = this.FarmComments,
                };

                base.StorageService.AddFarm(farm);
                base.StorageService.SetActiveFarm(farm);

                base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(ChooseComponentsView));
            }
        }

        #endregion

        #region Private Methods

        private void OnNavigateToPreviousPage()
        {
           base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(FarmOptionsView));
        }

        #endregion
    }
}
