using H.Core.Services.StorageService;
using Prism.Events;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class DefaultBeddingCompositionViewModel : ViewModelBase
    {
        public DefaultBeddingCompositionViewModel(
            IRegionManager regionManager,
            IEventAggregator eventAggregator,
            IStorageService storageService) : base(regionManager, eventAggregator, storageService)
        {
        }
    }
}
