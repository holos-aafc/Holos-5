using System;
using H.Core;
using H.Core.Enumerations;
using H.Core.Models.Animals;
using H.Core.Services.StorageService;


namespace H.Avalonia.ViewModels.ComponentViews.OtherAnimals
{
    public class GoatsComponentViewModelDesign : GoatsComponentViewModel
    {
        public GoatsComponentViewModelDesign()
        {
            ViewName = "Goats";
            OtherAnimalType = AnimalType.Goats;
            Groups.Add(new AnimalGroup { GroupType = AnimalType.Goats });
            ManagementPeriodViewModels.Add(new ManagementPeriodViewModel { PeriodName = "Test Group #1", StartDate = new DateTime(2000, 01, 01), EndDate = new DateTime(2001, 01, 01), NumberOfDays = 364 });
        }
    }
}
