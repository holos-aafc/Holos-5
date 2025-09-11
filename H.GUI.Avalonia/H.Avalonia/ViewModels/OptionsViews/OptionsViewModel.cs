using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using H.Avalonia.Events;
using H.Avalonia.Views.ComponentViews;
using H.Avalonia.Views.FarmCreationViews;
using H.Core.Models;
using H.Core.Services.StorageService;
using Prism.Events;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class OptionsViewModel : ViewModelBase
    {
        private object _selectedItem;
        public OptionsViewModel()
        {

        }
        public OptionsViewModel(IRegionManager regionManager) : base(regionManager)
        {
            this.PropertyChanged += OnSelectedOptionChanged;
        }

        public object SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }
        private void OnSelectedOptionChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (SelectedItem is ListBoxItem item && item.Content != null)
            {
                string? selectedOption = item.Content.ToString();
                switch (selectedOption)
                {
                    // File Menu
                    case "New Farm":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(Views.FarmCreationViews.FileNewFarmView));
                        break;
                    case "Open Farm":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(Views.FarmCreationViews.FileOpenFarmView));
                        break;

                    case "Close Farm":
                        this.ClearActiveView();
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(FarmOptionsView));
                        break;

                    case "Farms":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(Views.OptionsViews.FileMenuViews.FarmManagementView));
                        break;

                    case "Save Options":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(Views.OptionsViews.FileMenuViews.FileSaveOptionsView));
                        break;

                    case "Export Farm(s)":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(Views.OptionsViews.FileMenuViews.FileExportFarmView));
                        break;

                    case "Import Farm":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(Views.OptionsViews.FileMenuViews.FileImportFarmView));
                        break;

                    case "Export Climate":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(Views.OptionsViews.FileMenuViews.FileExportClimateView));
                        break;

                    case "Export Manure":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(Views.OptionsViews.FileMenuViews.FileExportManureView));
                        break;

                    // Settings Menu
                    case "Farm":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(Views.OptionsViews.OptionFarmView));
                        break;
                    case "Soil":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(Views.OptionsViews.OptionSoilView));
                        break;
                    case "Soil N2O Breakdown":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(Views.OptionsViews.OptionSoilN2OBreakdownView));
                        break;
                    case "Barn Temperatures":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(Views.OptionsViews.OptionBarnTemperatureView));
                        break;
                    case "Temperature":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(Views.OptionsViews.OptionTemperatureView));
                        break;
                    case "Precipitation":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(Views.OptionsViews.OptionPrecipitationView));
                        break;
                    case "Evapotranspiration":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(Views.OptionsViews.OptionEvapotranspirationView));
                        break;
                    case "Default Bedding Composition":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(Views.OptionsViews.DefaultBeddingCompositionView));
                        break;
                    case "Default Manure Composition":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(Views.OptionsViews.DefaultManureCompositionView));
                        break;

                    case "User Settings":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(Views.OptionsViews.OptionUserSettingsView));
                        break;
                }
            }
        }
        public void OnCancelExecute()
        {
            base.RegionManager.RequestNavigate(UiRegions.SidebarRegion, nameof(MyComponentsView));
            var activeView = this.RegionManager.Regions[UiRegions.ContentRegion].ActiveViews.SingleOrDefault();
            if (activeView != null)
            {
                this.RegionManager.Regions[UiRegions.ContentRegion].Deactivate(activeView);
                this.RegionManager.Regions[UiRegions.ContentRegion].Remove(activeView);
                SelectedItem = null; // need to set this to null because the option in the combo box stays selected otherwise
            }
        }

        #region Private Methods
        private void ClearActiveView()
        {
            // Clear content region
            var contentView = this.RegionManager.Regions[UiRegions.ContentRegion].ActiveViews.SingleOrDefault();
            if (contentView != null)
            {
                this.RegionManager.Regions[UiRegions.ContentRegion].Deactivate(contentView);
                this.RegionManager.Regions[UiRegions.ContentRegion].Remove(contentView);
            }

            // Clear sidebar region
            var sidebarView = this.RegionManager.Regions[UiRegions.SidebarRegion].ActiveViews.SingleOrDefault();
            if (sidebarView != null)
            {
                this.RegionManager.Regions[UiRegions.SidebarRegion].Deactivate(sidebarView);
                this.RegionManager.Regions[UiRegions.SidebarRegion].Remove(sidebarView);
            }
        }

        #endregion

    }
}
