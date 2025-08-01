using H.Core.Models;
using System.ComponentModel;
using H.Core.Services.StorageService;
using Prism.Regions;
using H.Avalonia.ViewModels.OptionsViews.DataTransferObjects;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class OptionSoilN2OBreakdownViewModel : ViewModelBase
    {
        #region Fields
        private SoilN2OBreakdownDTO _data;
        #endregion

        #region Constructors
        public OptionSoilN2OBreakdownViewModel() { }
        public OptionSoilN2OBreakdownViewModel(IStorageService storageService) : base(storageService)
        {

        }
        #endregion

        #region Properties
        public SoilN2OBreakdownDTO Data
        {
            get => _data;
            set => SetProperty(ref _data, value);
        } 
        #endregion

        #region Public Methods

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (!IsInitialized)
            {
                Data = Data = new SoilN2OBreakdownDTO(StorageService);
                IsInitialized = true;
            }
        }

        #endregion

        #region Event Handlers

        #endregion
    }
}
