
using H.Core.Services.StorageService;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class OptionUserSettingsViewModel : ViewModelBase
    {
        #region Constructors
        public OptionUserSettingsViewModel() { }
        public OptionUserSettingsViewModel(IStorageService storageService) : base(storageService)
        {
            Data = new UserSettingsDisplayViewModel(storageService);
        }
        #endregion
        #region Properties
        public UserSettingsDisplayViewModel Data { get; set; }
        #endregion
    }
}
