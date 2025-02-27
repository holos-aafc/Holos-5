using System;
using H.Core.Enumerations;
using H.Core.Models.Animals;
using H.Core.Services.StorageService;

namespace H.Avalonia.ViewModels.ComponentViews.OtherAnimals
{
    public class LlamaComponentViewModelDesign : LlamaComponentViewModel
    {
        public LlamaComponentViewModelDesign()
        {
            ViewName = "Llamas";
            OtherAnimalType = AnimalType.Llamas;
            Groups.Add(new AnimalGroup { GroupType = OtherAnimalType });
            ManagementPeriodViewModels.Add(new ManagementPeriodViewModel { PeriodName = "Test Group #1", StartDate = new DateTime(2000, 01, 01), EndDate = new DateTime(2001, 01, 01), NumberOfDays = 364 });
        }
    }
}
