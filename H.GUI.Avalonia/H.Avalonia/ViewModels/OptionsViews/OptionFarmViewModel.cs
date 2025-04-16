using H.Core.Services.StorageService;
using Mapsui.Extensions;

using NetTopologySuite.Geometries;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class OptionFarmViewModel : ViewModelBase
    {

        public OptionFarmViewModel() { }
        public OptionFarmViewModel(IStorageService storageService) : base(storageService)
        {
            Data = new FarmDisplayViewModel(storageService);
        }

        public FarmDisplayViewModel Data { get; set; }
        

    }
}
