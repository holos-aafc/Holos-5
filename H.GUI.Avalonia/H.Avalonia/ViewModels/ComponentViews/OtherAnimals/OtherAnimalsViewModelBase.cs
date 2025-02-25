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

        /// <summary>
        /// String used to refer to a particular other animals component, value set by child classes. Binded to the view(s), used as a title.
        /// Can be changed by the user, if they happen to leave it empty, an error will be thrown.
        /// </summary>
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

        /// <summary>
        ///  The <see cref="AnimalType"/> a respective component represents, used in the <see cref="Groups"/> collection / Groups data grid in the view(s), value set in child classes.
        /// </summary>
        public AnimalType OtherAnimalType
        {
            get => _otherAnimalType;
            set => SetProperty(ref _otherAnimalType, value);
        }

        /// <summary>
        /// An Observable Collection that holds <see cref="ManagementPeriod"/> objects, binded to a DataGrid in the view(s).
        /// </summary>
        public ObservableCollection<ManagementPeriod> ManagementPeriods
        {
            get => _managementPeriods;
            set => SetProperty(ref _managementPeriods, value);
        }

        /// <summary>
        /// An Observable Collection that holds <see cref="AnimalGroup"/> objects, binded to a DataGrid in the view(s).
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
        }

        /// <summary>
        ///  Binded to a button in the view, adds an item to the <see cref="Groups"/> collection / a row to the respective binded DataGrid. Seeded with <see cref="OtherAnimalType"/>.
        /// </summary>
        public void HandleAddGroupEvent()
        {
            Groups.Add(new AnimalGroup { GroupType = OtherAnimalType });
        }

        /// <summary>
        ///  Binded to a button in the view, adds an item to the <see cref="ManagementPeriods"/> collection / a row to the respective binded DataGrid. Seeded with some default values.
        /// </summary>
        public void HandleAddManagementPeriodEvent()
        {
            int numPeriods = ManagementPeriods.Count;
            var newManagementPeriod = new ManagementPeriod { GroupName = $"Period #{numPeriods}", Start = new DateTime(2024, 01, 01), End = new DateTime(2025, 01, 01), NumberOfDays = 364 };
            ManagementPeriods.Add(newManagementPeriod);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Ensures that a user cannot leave the <see cref="ViewName"/> empty when editing it in the UI. Uses INotifyDataErrorInfo implementation in <see cref="ViewModelBase"/>.
        /// </summary>
        private void ValidateViewName()
        {
            RemoveError(nameof(ViewName));

            if (string.IsNullOrEmpty(ViewName))
            {
                AddError(nameof(ViewName), "Name cannot be empty.");
                return;
            }
        }

        #endregion
    }
}