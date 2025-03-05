using H.Core.Services.StorageService;
using Prism.Events;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class DefaultManureCompositionViewModel : ViewModelBase
    {
        public DefaultManureCompositionViewModel(
            IRegionManager regionManager,
            IEventAggregator eventAggregator,
            IStorageService storageService) : base(regionManager, eventAggregator, storageService) 
        {
        }
    }
}
