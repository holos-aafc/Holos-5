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
            base.PropertyChanged += OnSelectedOptionChanged;
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
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(FileNewFarmView));
                        break;
                    case "Open Farm":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(FileOpenFarmView));
                        break;

                    case "Close Farm":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(FarmOptionsView));
                        break;

                    case "Farms":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(FarmManagementView));
                        break;

                    // Settings Menu
                    case "Farm":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(OptionFarmView));
                        break;
                    case "Soil":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(OptionSoilView));
                        break;
                    case "Soil N2O Breakdown":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(OptionSoilN2OBreakdownView));
                        break;
                    case "Barn Temperatures":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(OptionBarnTemperatureView));
                        break;
                    case "Temperature":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(OptionTemperatureView));
                        break;
                    case "Precipitation":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(OptionPrecipitationView));
                        break;
                    case "Evapotranspiration":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(OptionEvapotranspirationView));
                        break;
                    case "Default Bedding Composition":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(DefaultBeddingCompositionView));
                        break;
                    case "Default Manure Composition":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(DefaultManureCompositionView));
                        break;

                    case "User Settings":
                        base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(OptionUserSettingsView));
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

    }
}
