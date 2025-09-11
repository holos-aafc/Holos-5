using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H.Avalonia.ViewModels.FarmCreationViews;
using H.Core.Services.StorageService;
using H.Core.Services;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews.FileMenuViews
{
    public class FileNewFarmViewModel : FarmCreationViewModel
    {
        #region Constructors

        public FileNewFarmViewModel(IRegionManager regionManager, IStorageService storageService, IFarmHelper farmHelper) : base(regionManager, storageService, farmHelper)
        { 
        
        }

        #endregion

        #region Public Methods

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
        }

        #endregion
    }
}
