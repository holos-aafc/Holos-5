using H.Core.Services.StorageService;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class OptionSoilN2OBreakdownViewModel : ViewModelBase
    {
        #region Constructors
        public OptionSoilN2OBreakdownViewModel() { }
        public OptionSoilN2OBreakdownViewModel(IStorageService storageService) : base(storageService)
        {
            Data = new SoilN2OBreakdownDisplayViewModel(storageService);
        }
        #endregion
        #region Properties
        public SoilN2OBreakdownDisplayViewModel Data { get; set; }
        #endregion
    }
}
