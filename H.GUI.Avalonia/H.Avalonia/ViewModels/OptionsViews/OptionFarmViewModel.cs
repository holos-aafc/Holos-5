using H.Core.Services.StorageService;
using Mapsui.Extensions;

using NetTopologySuite.Geometries;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class OptionFarmViewModel : ViewModelBase
    {

        public OptionFarmViewModel() { }
        public OptionFarmViewModel(IRegionManager regionManager, IStorageService storageService) : base(regionManager, storageService)
        {
            ActiveFarm = base.StorageService.GetActiveFarm();
            Data = new FarmDisplayViewModel(storageService);
            Data.FarmName = ActiveFarm.Name;
            Data.FarmComments = ActiveFarm.Comments;
            Data.Coordinates = $"{ActiveFarm.Latitude}, {ActiveFarm.Longitude}";
            Data.GrowingSeasonPrecipitation = ActiveFarm.ClimateData.PrecipitationData.GrowingSeasonPrecipitation;
            Data.GrowingSeasonEvapotranspiration = ActiveFarm.ClimateData.EvapotranspirationData.GrowingSeasonEvapotranspiration;
        }

        public FarmDisplayViewModel Data { get; set; }
        

    }
}
