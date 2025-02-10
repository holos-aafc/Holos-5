using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H.Core.Enumerations;
using H.Core.Models.Animals;

namespace H.Avalonia.ViewModels.ComponentViews.OtherAnimals
{
    public class BisonComponentViewModelDesign : BisonComponentViewModel
    {
        public BisonComponentViewModelDesign() {
            base.ViewName = "Bison";
            base.Groups.Add(new AnimalGroup { GroupType = AnimalType.Bison });
            base.ManagementPeriods.Add(new ManagementPeriod { GroupName = "Test Group #1", Start = new DateTime(2000, 01, 01), End = new DateTime(2001, 01, 01), NumberOfDays = 364 });
        }
    }
}
