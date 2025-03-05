using H.Core.Providers.AnaerobicDigestion;
using H.Core.Providers.Temperature;
using H.Core.Services.StorageService;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class OptionBarnTemperatureViewModel : ViewModelBase
    {
        #region Fields

        private H.Core.Models.Farm _activeFarm;
        private TemperatureData _temperatureData;
        

        #endregion

        #region Constructors

        public OptionBarnTemperatureViewModel()
        {

        }

        public OptionBarnTemperatureViewModel(IRegionManager regionManager, IStorageService storageService) : base(regionManager, storageService)
        {

        }

        #endregion

        #region Properties
        
        public TemperatureData TemperatureData
        {
            get => _temperatureData;
            set => SetProperty(ref _temperatureData, value);
        }

        #endregion
    }
}
