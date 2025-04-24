using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class SelectOptionViewModel : ViewModelBase
    {
        private readonly IRegionManager _regionManager;
        public SelectOptionViewModel()
        {

        }
        public SelectOptionViewModel(IRegionManager regionManager) : base(regionManager)
        {
            _regionManager = regionManager ?? throw new System.ArgumentNullException(nameof(regionManager));
        }
    }
}
