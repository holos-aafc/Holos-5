using H.Core.Services.StorageService;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class OptionSoilN2OBreakdownViewModel : ViewModelBase
    {
        #region Constructors
        public OptionSoilN2OBreakdownViewModel() { }
        public OptionSoilN2OBreakdownViewModel(IRegionManager regionManager, IStorageService storageService) : base(regionManager, storageService)
        {
            ActiveFarm = base.StorageService.GetActiveFarm();
            Data = new SoilN2OBreakdownDisplayViewModel(storageService);
            Data.January = ActiveFarm.AnnualSoilN2OBreakdown.January;
            Data.February = ActiveFarm.AnnualSoilN2OBreakdown.February;
            Data.March = ActiveFarm.AnnualSoilN2OBreakdown.March;
            Data.April = ActiveFarm.AnnualSoilN2OBreakdown.April;
            Data.May = ActiveFarm.AnnualSoilN2OBreakdown.May;
            Data.June = ActiveFarm.AnnualSoilN2OBreakdown.June;
            Data.July = ActiveFarm.AnnualSoilN2OBreakdown.July;
            Data.August = ActiveFarm.AnnualSoilN2OBreakdown.August;
            Data.September = ActiveFarm.AnnualSoilN2OBreakdown.September;
            Data.October = ActiveFarm.AnnualSoilN2OBreakdown.October;
            Data.November = ActiveFarm.AnnualSoilN2OBreakdown.November;
            Data.December = ActiveFarm.AnnualSoilN2OBreakdown.December;
            Data.December = ActiveFarm.AnnualSoilN2OBreakdown.December;
        }
        #endregion
        #region Properties
        public SoilN2OBreakdownDisplayViewModel Data { get; set; }
        #endregion
    }
}
