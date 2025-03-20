using Prism.Regions;
using H.Core.Services.StorageService;
using H.Core.Providers.Precipitation;
using Prism.Events;
using DynamicData.Binding;
using H.Core.Enumerations;
using System;
using System.ComponentModel;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class OptionPrecipitationViewModel : ViewModelBase
    {
        #region Fields

        private PrecipitationData _bindingPrecipitationData = new PrecipitationData();

        #endregion

        #region Constructors

        public OptionPrecipitationViewModel()
        { 

        }

        public OptionPrecipitationViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IStorageService storageService) : base(regionManager, eventAggregator, storageService)
        {
            GetData();
            BindingPrecipitationData.PropertyChanged -= Data_PropertyChanged;
            BindingPrecipitationData.PropertyChanged += Data_PropertyChanged;
        }


        #endregion

        #region Properties
        
        public PrecipitationData BindingPrecipitationData
        {
            get => _bindingPrecipitationData;
            set => SetProperty(ref _bindingPrecipitationData, value);  
        }
       public double January
        {
            get => BindingPrecipitationData.January;
            set
            {
                ValidateValue(value, nameof(January));
                if(HasErrors)
                {
                    return;
                }
                BindingPrecipitationData.January = value;
                RaisePropertyChanged(nameof(January));
            }
        }
        #endregion

        #region Public Methods

        public void GetData()
        {
            BindingPrecipitationData = ActiveFarm.ClimateData.PrecipitationData;
            // This format was what was causing the event handler in the code behind to not fire.
            /* BindingPrecipitationData.January = ActiveFarm.ClimateData.PrecipitationData.January;
            BindingPrecipitationData.February = ActiveFarm.ClimateData.PrecipitationData.February;
            BindingPrecipitationData.March = ActiveFarm.ClimateData.PrecipitationData.March;
            BindingPrecipitationData.April = ActiveFarm.ClimateData.PrecipitationData.April;
            ... */  
        }
        public void ValidateValue(double value, string propertyName)
        {
            if (value < 0)
            {
                AddError(propertyName, "Value must be greater than 0");
            }
            else
            {
                RemoveError(propertyName);
            }
        }
        private void Data_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is PrecipitationData)
            {
                ActiveFarm.ClimateData.PrecipitationData = BindingPrecipitationData;
                
            }
        }

        #endregion

    }
}
