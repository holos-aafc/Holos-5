using System;
using System.Collections.ObjectModel;
using H.Core.Enumerations;
using H.Core.Models;
using H.Core.Models.Animals;
using H.Core.Services.StorageService;
using Prism.Regions;

namespace H.Avalonia.ViewModels.ComponentViews.OtherAnimals
{
    public abstract class OtherAnimalsViewModelBase : ViewModelBase
    {
        #region Fields

        private AnimalType _otherAnimalType;
        private ObservableCollection<AnimalGroup> _animalGroups;
        private ObservableCollection<ManagementPeriodViewModel> _managementPeriodViewModels;

        #endregion

        #region Constructors

        public OtherAnimalsViewModelBase(IStorageService storageService) : base(storageService)
        {
            ManagementPeriodViewModels = new ObservableCollection<ManagementPeriodViewModel>();
            Groups = new ObservableCollection<AnimalGroup>();
        }
        public OtherAnimalsViewModelBase() 
        { 
        }

        #endregion

        #region Properties

        /// <summary>
        ///  The <see cref="AnimalType"/> a respective component represents, used in the <see cref="Groups"/> collection / Groups data grid in the view(s), value set in child classes.
        /// </summary>
        public AnimalType OtherAnimalType
        {
            get => _otherAnimalType;
            set => SetProperty(ref _otherAnimalType, value);
        }

        /// <summary>
        /// An Observable Collection that holds <see cref="ManagementPeriodViewModel"/> objects, bound to a DataGrid in the view(s).
        /// </summary>
        public ObservableCollection<ManagementPeriodViewModel> ManagementPeriodViewModels
        {
            get => _managementPeriodViewModels;
            set => SetProperty(ref _managementPeriodViewModels, value);
        }

        /// <summary>
        /// An Observable Collection that holds <see cref="AnimalGroup"/> objects, bound to a DataGrid in the view(s).
        /// </summary>
        public ObservableCollection<AnimalGroup> Groups
        {
            get => _animalGroups;
            set => SetProperty(ref _animalGroups, value);
        }

        #endregion

        #region Public Methods

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            AddExistingManagementPeriods();
        }

        /// <summary>
        /// Adds <see cref="ManagementPeriodViewModels"/> that correspond to existing <see cref="ManagementPeriod"/> objects associated with the current <see cref="Farm"/>
        /// </summary>
        public void AddExistingManagementPeriods()
        {
            Farm currentFarm = StorageService.GetActiveFarm();
            var existingManagementPeriods = currentFarm.GetAllManagementPeriods();
            foreach(var managementPeriod in existingManagementPeriods)
            {
                var newManagementPeriodViewModel = new ManagementPeriodViewModel();
                newManagementPeriodViewModel.PeriodName = managementPeriod.GroupName;
                newManagementPeriodViewModel.StartDate = managementPeriod.Start;
                newManagementPeriodViewModel.EndDate = managementPeriod.End;
                newManagementPeriodViewModel.NumberOfDays = managementPeriod.NumberOfDays;
                ManagementPeriodViewModels.Add(newManagementPeriodViewModel);
            }
        }

        /// <summary>
        ///  bound to a button in the view, adds an item to the <see cref="Groups"/> collection / a row to the respective bound DataGrid. Seeded with <see cref="OtherAnimalType"/>.
        /// </summary>
        public void HandleAddGroupEvent()
        {
            Groups.Add(new AnimalGroup { GroupType = OtherAnimalType });
        }

        /// <summary>
        ///  bound to a button in the view, adds an item to the <see cref="ManagementPeriodViewModels"/> collection / a row to the respective bound DataGrid. Seeded with some default values.
        /// </summary>
        public void HandleAddManagementPeriodEvent()
        {
            int numPeriods = ManagementPeriodViewModels.Count;
            var newManagementPeriodViewModel = new ManagementPeriodViewModel { PeriodName = $"Period #{numPeriods}", StartDate = new DateTime(2024, 01, 01), EndDate = new DateTime(2025, 01, 01), NumberOfDays = 364};
            ManagementPeriodViewModels.Add(newManagementPeriodViewModel);
        }

        #endregion

        #region Private Methods

        #endregion
    }
}