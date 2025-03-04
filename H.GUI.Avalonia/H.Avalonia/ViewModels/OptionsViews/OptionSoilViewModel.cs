using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class OptionSoilViewModel : ViewModelBase
    {
        private H.Core.Models.Farm _activeFarm;
        public OptionSoilViewModel() { }
        public OptionSoilViewModel(IRegionManager regionManager) : base(regionManager)
        {
        }
        public H.Core.Models.Farm ActiveFarm
        {
            get => _activeFarm;
            set => SetProperty(ref _activeFarm, value);
        }
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            ActiveFarm = base.StorageService.GetActiveFarm();
            
            base.OnNavigatedTo(navigationContext);
        }
    }
}
