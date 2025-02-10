using System;
using System.Collections.ObjectModel;
using H.Core.Models.Animals;
using H.Core.Enumerations;
using Prism.Regions;

namespace H.Avalonia.ViewModels.ComponentViews.OtherAnimals
{
    public class HorsesComponentViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<ManagementPeriod> _managementPeriods;
        private ObservableCollection<AnimalGroup> _animalGroups;

        #endregion

        #region Constructors
        public HorsesComponentViewModel() 
        {
            ViewName = "Horses";
            Groups = new ObservableCollection<AnimalGroup>();
            ManagementPeriods = new ObservableCollection<ManagementPeriod>();
        }

        #endregion

        #region Properties

        public string ViewName { get; set; }

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
            Groups.Add(new AnimalGroup { GroupType = AnimalType.Horses });
        }

        public void HandleAddManagementPeriodEvent()
        {
            int numPeriods = ManagementPeriods.Count;
            ManagementPeriods.Add(new ManagementPeriod { GroupName = $"Period #{numPeriods}", Start = new DateTime(2020, 03, 13), End = new DateTime(2021, 03, 13), NumberOfDays = 364 });
        }
        #endregion
    }
}