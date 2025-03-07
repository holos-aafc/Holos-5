using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H.Core.Services.StorageService;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class OptionSoilN2OBreakdownViewModel : ViewModelBase
    {
        public OptionSoilN2OBreakdownViewModel() { }
        public OptionSoilN2OBreakdownViewModel(IRegionManager regionManager, IStorageService storageService) : base(regionManager, storageService)
        {
            ActiveFarm = base.StorageService.GetActiveFarm();
        }
    }
}
