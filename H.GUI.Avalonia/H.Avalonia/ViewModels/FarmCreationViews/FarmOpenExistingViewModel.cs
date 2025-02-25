using H.Avalonia.Views.FarmCreationViews;
using Prism.Commands;
using Prism.Regions;
using System.Windows.Input;
using H.Core.Services.StorageService;
using System.Collections.ObjectModel;
using DynamicData;
using H.Avalonia.Models;
using System;
using Mapsui.Utilities;
using Avalonia.Controls;
using System.Linq;
using H.Core.Models.Animals;
using H.Core.Models;
using H.Avalonia.Views.ComponentViews;
using H.Avalonia.Events;

namespace H.Avalonia.ViewModels
{
    public class FarmOpenExistingViewmodel : ViewModelBase
    {
        #region Fields
        private readonly IRegionManager _regionManager;
        private H.Core.Models.Farm _selectedFarm;

        #endregion

        #region Constructors
        public FarmOpenExistingViewmodel()
        {
            
        }
        public FarmOpenExistingViewmodel(IRegionManager regionManager, IStorageService storageService) : base(regionManager, storageService)
        {
            _regionManager = regionManager ?? throw new System.ArgumentNullException(nameof(regionManager));
            NavigateToPreviousPage = new DelegateCommand(OnNavigateToPreviousPage);
            Farms = new ObservableCollection<H.Core.Models.Farm>();
        }
        #endregion

        #region Properties
        public ICommand NavigateToPreviousPage { get; }
        public ObservableCollection<H.Core.Models.Farm> Farms { get; set; }
        public H.Core.Models.Farm SelectedFarm
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
            base.StorageService.SetActiveFarm(this.SelectedFarm);
            base.RegionManager.RequestNavigate(UiRegions.SidebarRegion, nameof(MyComponentsView));

            var view = this.RegionManager.Regions[UiRegions.ContentRegion].ActiveViews.Single();
            this.RegionManager.Regions[UiRegions.ContentRegion].Deactivate(view);
            this.RegionManager.Regions[UiRegions.ContentRegion].Remove(view);

        }
        #endregion
    }
}
