using System;
using H.Avalonia.Views.FarmCreationViews;
using Prism.Commands;
using Prism.Regions;
using System.Windows.Input;
using H.Avalonia.Views.ComponentViews;
using H.Core.Models;
using H.Core.Services.StorageService;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using H.Core.Services;

namespace H.Avalonia.ViewModels
{
    public class FarmCreationViewModel : ViewModelBase
    {
        
        #region Fields

        private string _farmName;
        private string _farmComments;
        private readonly IFarmHelper _farmHelper;

        #endregion

        #region Constructors

        public FarmCreationViewModel()
        {
        }

        public FarmCreationViewModel(IRegionManager regionManager, IStorageService storageService, IFarmHelper farmHelper) : base(regionManager, storageService)
        {
            if (farmHelper != null)
            {
                _farmHelper = farmHelper;
            }
            else
            {
                throw new ArgumentNullException(nameof(farmHelper));
            }

            NavigateToPreviousPage = new DelegateCommand(OnNavigateToPreviousPage);
        }

        #endregion

        #region Properties

        public ICommand NavigateToPreviousPage { get; }

        public string FarmName
        {
            get => _farmName;
            set
            {
                if (SetProperty(ref _farmName, value))
                {    
                    ValidateFarmName();
                }
            }
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
            // Validate FarmName before proceeding
            ValidateFarmName();

            if (HasErrors)
            {
                // Optionally notify the user that there are validation errors
                return;
            }

            var farm = _farmHelper.Create();
            farm.Name = FarmName;
            farm.Comments = FarmComments;

            base.StorageService.AddFarm(farm);
            base.StorageService.SetActiveFarm(farm);
            base.StorageService.Storage.ApplicationData.DisplayUnitStrings.SetStrings(farm.MeasurementSystemType);

            RegionManager.RequestNavigate(UiRegions.SidebarRegion, nameof(MyComponentsView));
            RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(ChooseComponentsView));
        }

        #endregion

        #region Private Methods

        private void OnNavigateToPreviousPage()
        {
           base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(FarmOptionsView));
        }
        private void ValidateFarmName()
        {
            const string propertyName = nameof(FarmName);

            if (string.IsNullOrWhiteSpace(_farmName))
            {
                AddError(propertyName, "Farm name cannot be empty.");
            }
            else
            {
                RemoveError(propertyName);
            }
        }
        #endregion
    }
}
