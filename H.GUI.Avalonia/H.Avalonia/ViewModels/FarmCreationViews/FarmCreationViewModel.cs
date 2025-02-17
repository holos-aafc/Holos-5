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

namespace H.Avalonia.ViewModels
{
    public class FarmCreationViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        #region Fields

        private string _farmName;
        private string _farmComments;
        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();


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

        public bool HasErrors => _errors.Any();

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

            var farm = new Farm()
            {
                Name = this.FarmName,
                Comments = this.FarmComments,
            };

            base.StorageService.AddFarm(farm);
            base.StorageService.SetActiveFarm(farm);

            base.RegionManager.RequestNavigate(UiRegions.SidebarRegion, nameof(MyComponentsView));
            base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(ChooseComponentsView));
        }

        #endregion

        #region Private Methods

        private void OnNavigateToPreviousPage()
        {
           base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(FarmOptionsView));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName) || !_errors.ContainsKey(propertyName))
            {
                return null;
            }
            return _errors[propertyName];
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

        private void AddError(string propertyName, string error)
        {
            if (!_errors.ContainsKey(propertyName))
            {
                _errors[propertyName] = new List<string>();
            }

            if (!_errors[propertyName].Contains(error))
            {
                _errors[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        private void RemoveError(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
            {
                _errors[propertyName].Clear();
                _errors.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        IEnumerable INotifyDataErrorInfo.GetErrors(string? propertyName)
        {
            return GetErrors(propertyName);
        }

        #endregion
    }
}
