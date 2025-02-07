using System;
using H.Core.Models.Animals;
using H.Core.Enumerations;

namespace H.Avalonia.ViewModels.ComponentViews.OtherAnimals
{
    public class HorsesComponentViewModelDesign : HorsesComponentViewModel
    {
        public HorsesComponentViewModelDesign() 
        {
            base.ViewName = "Horses";
            base.Groups.Add(new AnimalGroup { GroupType = AnimalType.Horses });
            base.ManagementPeriods.Add(new ManagementPeriod { GroupName = "Test Group #1", Start = new DateTime(2000,01,01), End = new DateTime(2001,01,01), NumberOfDays=364});
        }
    }
}
