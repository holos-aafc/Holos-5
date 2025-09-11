using H.Core.Services;
using H.Core.Services.StorageService;
using Prism.Regions;

namespace H.Avalonia.ViewModels.FarmCreationViews;

public class FarmCreationViewModelDesign : FarmCreationViewModel
{
    #region Constructors

    public FarmCreationViewModelDesign()
    {
    }

    public FarmCreationViewModelDesign(IRegionManager regionManager, IStorageService storageService, IFarmHelper farmHelper) : base(regionManager, storageService, farmHelper)
    {
    }

    #endregion
}