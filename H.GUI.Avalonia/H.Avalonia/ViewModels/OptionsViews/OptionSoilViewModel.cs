using System.Collections.ObjectModel;
using H.Core.Enumerations;
using H.Core.Services.StorageService;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class OptionSoilViewModel : ViewModelBase
    {
        #region Constructors
        public OptionSoilViewModel() { }
        public OptionSoilViewModel(IRegionManager regionManager, IStorageService storageService) : base(regionManager, storageService)
        {
            SoilTextures = new ObservableCollection<SoilTexture>();
            ActiveFarm = base.StorageService.GetActiveFarm();
            var soilTextures = SoilTexture.GetValues(typeof(SoilTexture));
            foreach (SoilTexture soilTexture in soilTextures)
            {
                if (!SoilTextures.Contains(soilTexture))
                {
                    SoilTextures.Add(soilTexture);
                }
            }
            Data = new SoilDisplayViewModel(storageService);
            Data.BulkDensity = ActiveFarm.DefaultSoilData.BulkDensity;
            Data.TopLayerThickness = ActiveFarm.DefaultSoilData.TopLayerThickness;
            Data.ProportionOfClayInSoil = ActiveFarm.DefaultSoilData.ProportionOfClayInSoil;
            Data.SelectedSoilTexture = ActiveFarm.DefaultSoilData.SoilTexture;
            Data.ProportionOfSandInSoil = ActiveFarm.DefaultSoilData.ProportionOfSandInSoil;
            Data.ProportionOfSoilOrganicCarbon = ActiveFarm.DefaultSoilData.ProportionOfSoilOrganicCarbon;
            Data.SoilPh = ActiveFarm.DefaultSoilData.SoilPh;
            Data.CarbonModellingEquilibriumYear = ActiveFarm.CarbonModellingEquilibriumYear;
        }
        #endregion
        #region Properties
        public SoilDisplayViewModel Data { get; set; }
        public ObservableCollection<SoilTexture> SoilTextures { get; set; }
        #endregion
    }
}
