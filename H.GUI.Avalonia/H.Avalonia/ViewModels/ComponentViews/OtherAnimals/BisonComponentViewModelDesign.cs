using System;
using H.Core.Enumerations;
using H.Core.Models.Animals;

namespace H.Avalonia.ViewModels.ComponentViews.OtherAnimals
{
    public class BisonComponentViewModelDesign : BisonComponentViewModel
    {
        public BisonComponentViewModelDesign() 
        {
            ViewName = "Bison";
            OtherAnimalType = AnimalType.Bison;
            Groups.Add(new AnimalGroup { GroupType = OtherAnimalType });
            ManagementPeriods.Add(new ManagementPeriod { GroupName = "Test Group #1", Start = new DateTime(2000, 01, 01), End = new DateTime(2001, 01, 01), NumberOfDays = 364 });
        }
    }
}