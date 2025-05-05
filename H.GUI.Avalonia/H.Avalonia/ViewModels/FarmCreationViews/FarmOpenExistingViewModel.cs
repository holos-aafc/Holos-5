using H.Avalonia.Views.FarmCreationViews;
using Prism.Commands;
using Prism.Regions;
using System.Windows.Input;
using H.Core.Services.StorageService;
using System.Collections.ObjectModel;
using DynamicData;
using System.Linq;
using H.Avalonia.Views.ComponentViews;
using H.Core.Models;

namespace H.Avalonia.ViewModels
{
    public class FarmOpenExistingViewmodel : ViewModelBase
    {
        #region Fields
        private readonly IRegionManager _regionManager;
        private Farm _selectedFarm;

        #endregion

        #region Constructors
        public FarmOpenExistingViewmodel()
        {
            
        }
        public FarmOpenExistingViewmodel(IRegionManager regionManager, IStorageService storageService) : base(regionManager, storageService)
        {
            _regionManager = regionManager ?? throw new System.ArgumentNullException(nameof(regionManager));
            NavigateToPreviousPage = new DelegateCommand(OnNavigateToPreviousPage);
            Farms = new ObservableCollection<Farm>();
        }
        #endregion

        #region Properties
        public ICommand NavigateToPreviousPage { get; }
        public ObservableCollection<Farm> Farms { get; set; }
        public Farm SelectedFarm
        {
            get => _selectedFarm;
            set => SetProperty(ref _selectedFarm, value);
        }
        #endregion

        #region Public Methods

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            Farms.Clear();
            var farms = base.StorageService.GetAllFarms();
            Farms.Add(farms);
            base.OnNavigatedTo(navigationContext);
        }

        #endregion

        #region Private Methods

        private void OnNavigateToPreviousPage()
        {
            _regionManager.RequestNavigate(UiRegions.ContentRegion, nameof(FarmOptionsView));
        }

        public void OnOpenFarmExecute()
        {
            var testFarm = base.StorageService.GetActiveFarm();
            base.StorageService.SetActiveFarm(this.SelectedFarm);
            var testFarm2 = base.StorageService.GetActiveFarm();
            // Line below ensures that the proper unit strings are used for the MeasurementSystemType of the existing farm being opened
            base.StorageService.Storage.ApplicationData.DisplayUnitStrings.SetStrings(this.SelectedFarm.MeasurementSystemType);

            base.RegionManager.RequestNavigate(UiRegions.SidebarRegion, nameof(MyComponentsView));
            var view = this.RegionManager.Regions[UiRegions.ContentRegion].ActiveViews.Single();
            this.RegionManager.Regions[UiRegions.ContentRegion].Deactivate(view);
            this.RegionManager.Regions[UiRegions.ContentRegion].Remove(view);
        }

        #endregion
    }
}
