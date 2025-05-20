using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H.Core.Services.StorageService;
using Prism.Regions;

namespace H.Avalonia.ViewModels.FarmCreationViews
{
    public class FileOpenFarmViewModel : FarmOpenExistingViewmodel
    {
        #region Constructors

        public FileOpenFarmViewModel(IRegionManager regionManager, IStorageService storageService) : base(regionManager, storageService)
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
