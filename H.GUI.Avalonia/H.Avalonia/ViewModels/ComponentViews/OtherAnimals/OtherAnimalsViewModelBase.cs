using System;
using System.Collections.ObjectModel;
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
            Groups = new ObservableCollection<AnimalGroup>();
        }

        #endregion

        #region Properties

        public string ViewName
        {
            get => _viewName;
            set => SetProperty(ref _viewName, value);
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
            ManagementPeriods.Add(new ManagementPeriod { GroupName = $"Period #{numPeriods}", Start = new DateTime(2020, 03, 13), End = new DateTime(2021, 03, 13), NumberOfDays = 364 });
        }
        
        #endregion
    }
}
