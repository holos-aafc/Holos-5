using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using H.Core.Enumerations;
using H.Core.Models.Animals;
using Prism.Regions;

namespace H.Avalonia.ViewModels.ComponentViews.OtherAnimals
{
    public class OtherAnimalsViewModelBase : ViewModelBase
    {
        #region Fields

        private string _viewName;
        private AnimalType _otherAnimalType;
        private ObservableCollection<ManagementPeriod> _managementPeriods;
        private ObservableCollection<AnimalGroup> _animalGroups;

        #endregion

        #region Constructors

        public OtherAnimalsViewModelBase() 
        { 
            ManagementPeriods = new ObservableCollection<ManagementPeriod>();
            //ManagementPeriods.CollectionChanged += ManagementPeriodCollectionChanged;
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

        //public string TestCollectionError 
        //{ 
        //    get
        //    {
        //        List<string> errors = (List<string>)GetErrors(nameof(ManagementPeriods));
        //        return errors[0];
        //    }
        
        //}

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
            var newManagementPeriod = new ManagementPeriod { GroupName = $"Period #{numPeriods}", Start = new DateTime(2024, 01, 01), End = new DateTime(2025, 01, 01), NumberOfDays = 364 };
            //newManagementPeriod.PropertyChanged += ManagementPeriodPropertyChanged;
            ManagementPeriods.Add(newManagementPeriod);
            //ValidateManagementPeriodCollection();
        }

        #endregion

        #region Validation Logic

        private void ValidateViewName()
        {
            RemoveError(nameof(ViewName));

            if (string.IsNullOrEmpty(ViewName))
            {
                AddError(nameof(ViewName), "Name cannot be empty.");
                return;
            }
        }

        private void ValidateManagementPeriodCollection()
        {
            if (ManagementPeriods.Count > 2)
            {
                Debug.WriteLine("Here");
                AddError(nameof(ManagementPeriods), "Cannot exceed one management period");
                Debug.WriteLine("Stored error");
            }

            foreach (var error in GetErrors(nameof(ManagementPeriods)))
            {
                Debug.WriteLine(error);
            }

            //foreach (ManagementPeriod period in ManagementPeriods)
            //{
            //    if (string.IsNullOrEmpty(period.GroupName))
            //    {
            //        AddError(nameof(ManagementPeriods), "Name cannot be empty.");
            //    }

            //    int dateCompareResult = DateTime.Compare(period.Start, period.End);

            //    if (dateCompareResult == 0)
            //    {
            //        AddError(nameof(ManagementPeriods), "Start and End dates must be different");
            //    }

            //    if (dateCompareResult > 0)
            //    {
            //        AddError(nameof(ManagementPeriods), "End date must be later than the start date");
            //    }

            //    if (period.NumberOfDays <= 0)
            //    {
            //        AddError(nameof(ManagementPeriods), "Number of Days must be greater than 0");
            //    }
            //}

            OnErrorsChanged(nameof(ManagementPeriods));
            this.RaisePropertyChanged(nameof(HasErrors));
            this.RaisePropertyChanged(nameof(ManagementPeriods));
        }

        private void ManagementPeriodCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            ValidateManagementPeriodCollection();
        }

        private void ManagementPeriodPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ValidateManagementPeriodCollection();
        }

        #endregion
    }
}