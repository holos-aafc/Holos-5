using H.Core.Services.StorageService;
using Prism.Regions;

namespace H.Avalonia.ViewModels;

public class FarmCreationViewModelDesign : FarmCreationViewModel
{
    #region Constructors

    public FarmCreationViewModelDesign()
    {
    }

    public FarmCreationViewModelDesign(IRegionManager regionManager, IStorageService storageService) : base(regionManager, storageService)
    {
    } 

    #endregion
}