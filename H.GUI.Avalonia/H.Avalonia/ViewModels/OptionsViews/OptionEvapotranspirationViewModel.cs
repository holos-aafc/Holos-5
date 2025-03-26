using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H.Core.Providers.Evapotranspiration;
using H.Core.Services.StorageService;
using Prism.Events;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class OptionEvapotranspirationViewModel : ViewModelBase
    {
        #region Fields
        private EvapotranspirationData _bindingEvapotranspirationData = new EvapotranspirationData();
        #endregion
        #region Constructors
        public OptionEvapotranspirationViewModel()
        {
        }
        public OptionEvapotranspirationViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IStorageService storageService) : base(regionManager, eventAggregator, storageService)
        {
            BindingEvapotranspirationData = ActiveFarm.ClimateData.EvapotranspirationData;
            BindingEvapotranspirationData.PropertyChanged -= BindingEvapotranspirationData_PropertyChanged;
            BindingEvapotranspirationData.PropertyChanged += BindingEvapotranspirationData_PropertyChanged;
        }
        #endregion
        #region Properties
        public EvapotranspirationData BindingEvapotranspirationData 
        {
            get => _bindingEvapotranspirationData;
            set => SetProperty(ref _bindingEvapotranspirationData, value);
        }
        #endregion
        #region Event Handlers
        public void BindingEvapotranspirationData_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(sender is EvapotranspirationData evapotranspirationData)
            {
                ActiveFarm.ClimateData.EvapotranspirationData = evapotranspirationData;
            }
        }
        #endregion
    }
}
