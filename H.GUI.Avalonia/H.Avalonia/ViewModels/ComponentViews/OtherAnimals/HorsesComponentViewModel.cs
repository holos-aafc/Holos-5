using System;
using System.Collections.ObjectModel;
using H.Core.Models.Animals;
using Prism.Regions;

namespace H.Avalonia.ViewModels.ComponentViews.OtherAnimals
{
    public class HorsesComponentViewModel : ViewModelBase
    {
        public string ViewName { get; set; }

        public ObservableCollection<ManagementPeriod> ManagementPeriods { get; set; }

        public HorsesComponentViewModel() 
        {
            ViewName = "Horses";

            ManagementPeriods = new ObservableCollection<ManagementPeriod>
            {
                new ManagementPeriod {GroupName = "Period #1", Start=new DateTime(2020,03,13), End= new DateTime(2021,03,13), NumberOfDays=364},
                new ManagementPeriod {GroupName = "Period #2", Start=new DateTime(2022,12,12), End= new DateTime(2023,12,12), NumberOfDays=364}
            };
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
        }
    }
}