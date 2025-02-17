using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using H.Core.Enumerations;
using H.Core.Models.Animals;
using Prism.Regions;

namespace H.Avalonia.ViewModels.ComponentViews.OtherAnimals
{
    public class OtherAnimalsViewModelBase : ViewModelBase, INotifyDataErrorInfo
    {
        #region Fields

        private string _viewName;
        private AnimalType _otherAnimalType;
        private ObservableCollection<ManagementPeriod> _managementPeriods;
        private ObservableCollection<AnimalGroup> _animalGroups;

        private Dictionary<string, List<ValidationResult>> _errors = new Dictionary<string, List<ValidationResult>>();

        #endregion

        #region Constructors

        public OtherAnimalsViewModelBase() 
        { 
            ManagementPeriods = new ObservableCollection<ManagementPeriod>();
            Groups = new ObservableCollection<AnimalGroup>();
        }

        #endregion

        #region Properties

        public string ViewName
        {
            get => _viewName;
            set 
            {
                if (SetProperty(ref _viewName, value))
                {
                    ValidateViewName();
                }
            }
        }

        public AnimalType OtherAnimalType
        {
            get => _otherAnimalType;
            set => SetProperty(ref _otherAnimalType, value);
        }

        public ObservableCollection<ManagementPeriod> ManagementPeriods
        {
            get => _managementPeriods;
            set => SetProperty(ref _managementPeriods, value);
        }
        public ObservableCollection<AnimalGroup> Groups
        {
            get => _animalGroups;
            set => SetProperty(ref _animalGroups, value);
        }

        #endregion

        #region INotifyDataErrorInfo Implementation

        // Using code from avalonia docs

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public bool HasErrors => _errors.Count > 0;

        public IEnumerable GetErrors(string? propertyName)
        {
            // return list of all errors if propertyName is null/empty
            if (string.IsNullOrEmpty(propertyName))
            {
                List<ValidationResult> allErrors = new List<ValidationResult>();
                foreach (var dictionaryEntry  in _errors)
                {
                    foreach (ValidationResult error in dictionaryEntry.Value)
                    {
                        allErrors.Add(error);
                    }
                }
                return allErrors;
            }

            // if a specifc propertyName is given, check for associated errors
            if (_errors.ContainsKey(propertyName))
            {
                return _errors[propertyName];
            }

            // In case there are no errors we return an empty array.
            return Array.Empty<ValidationResult>();
        }

        protected void ClearErrors(string? propertyName = null)
        {
            // Clear everything if propertyName is null/empty
            if (string.IsNullOrEmpty(propertyName))
            {
                _errors.Clear();
            }
            // remove errors for a specifc property if propertyName given
            else
            {
                _errors.Remove(propertyName);
            }

            // Notify that errors have changed
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            this.RaisePropertyChanged(nameof(HasErrors));
        }

        protected void AddError(string propertyName, string errorMessage)
        {
            List<ValidationResult> propertyErrors;

            if (_errors.ContainsKey(propertyName))
            {
                propertyErrors = _errors[propertyName];
            }
            else
            {
                propertyErrors = new List<ValidationResult>();
                _errors.Add(propertyName, propertyErrors);
            }

            propertyErrors.Add(new ValidationResult(errorMessage));

            // Notify that errors have changed
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            this.RaisePropertyChanged(nameof(HasErrors));
        }

        private void ValidateViewName()
        {
            ClearErrors(nameof(ViewName));

            if (string.IsNullOrEmpty(ViewName))
            {
                AddError(nameof(ViewName), "Name cannot be empty.");
                return;
            }
        }

        #endregion

        #region Public Methods

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
        }

        public void HandleAddGroupEvent()
        {
            Groups.Add(new AnimalGroup { GroupType = OtherAnimalType });
        }

        public void HandleAddManagementPeriodEvent()
        {
            int numPeriods = ManagementPeriods.Count;
            ManagementPeriods.Add(new ManagementPeriod { GroupName = $"Period #{numPeriods}", Start = new DateTime(2024, 01, 01), End = new DateTime(2025, 01, 01), NumberOfDays = 364 });
        }

        #endregion
    }
}
