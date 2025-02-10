using H.Core.Enumerations;
using H.Core.Models.Animals;
using System.Collections.ObjectModel;
using System;

namespace H.Avalonia.ViewModels.ComponentViews.OtherAnimals
{
    public class BisonComponentViewModel : ViewModelBase
    {
        public string ViewName { get; set; }
        public ObservableCollection<ManagementPeriod> ManagementPeriods { get; set; }
        public ObservableCollection<AnimalGroup> Groups { get; set; }
        public BisonComponentViewModel()
        {
            ViewName = "Bison";

            //populating dummy data
            Groups = new ObservableCollection<AnimalGroup>
            {
                new AnimalGroup {GroupType = AnimalType.Bison}
            };

            ManagementPeriods = new ObservableCollection<ManagementPeriod>
            {
                new ManagementPeriod {GroupName = "Period #1", Start = new DateTime(2020,03,13), End = new DateTime(2021,03,13), NumberOfDays=364},
                new ManagementPeriod {GroupName = "Period #2", Start = new DateTime(2022,12,12), End = new DateTime(2023,12,12), NumberOfDays=364}
            };
        }
    }
}
