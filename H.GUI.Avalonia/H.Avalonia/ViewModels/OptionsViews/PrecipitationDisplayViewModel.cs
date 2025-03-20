using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H.Core.Enumerations;
using H.Core.Providers.Precipitation;
using H.Core.Services.StorageService;
using Prism.Events;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class PrecipitationDisplayViewModel : ViewModelBase
    {
        #region Fields
        private PrecipitationData _bindingPrecipitationData = new PrecipitationData();
        #endregion
        #region Constructors
        public PrecipitationDisplayViewModel(IStorageService storageService) : base(storageService)
        {
            ActiveFarm = base.StorageService.GetActiveFarm();
            ManageData();
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
                if (HasErrors)
                {
                    return;
                }
                BindingPrecipitationData.January = value;
                RaisePropertyChanged(nameof(January));
            }
        }
        public double February
        {
            get => BindingPrecipitationData.February;
            set
            {
                ValidateValue(value, nameof(February));
                if (HasErrors)
                {
                    return;
                }
                BindingPrecipitationData.February = value;
                RaisePropertyChanged(nameof(February));
            }
        }
        public double March
        {
            get => BindingPrecipitationData.March;
            set
            {
                ValidateValue(value, nameof(March));
                if (HasErrors)
                {
                    return;
                }
                BindingPrecipitationData.March = value;
                RaisePropertyChanged(nameof(March));
            }
        }
        public double April
        {
            get => BindingPrecipitationData.April;
            set
            {
                ValidateValue(value, nameof(April));
                if (HasErrors)
                {
                    return;
                }
                BindingPrecipitationData.April = value;
                RaisePropertyChanged(nameof(April));
            }
        }
        public double May
        {
            get => BindingPrecipitationData.May;
            set
            {
                ValidateValue(value, nameof(May));
                if (HasErrors)
                {
                    return;
                }
                BindingPrecipitationData.May = value;
                RaisePropertyChanged(nameof(May));
            }
        }
        public double June
        {
            get => BindingPrecipitationData.June;
            set
            {
                ValidateValue(value, nameof(June));
                if (HasErrors)
                {
                    return;
                }
                BindingPrecipitationData.June = value;
                RaisePropertyChanged(nameof(June));
            }
        }
        public double July
        {
            get => BindingPrecipitationData.July;
            set
            {
                ValidateValue(value, nameof(July));
                if (HasErrors)
                {
                    return;
                }
                BindingPrecipitationData.July = value;
                RaisePropertyChanged(nameof(July));
            }
        }
        public double August
        {
            get => BindingPrecipitationData.August;
            set
            {
                ValidateValue(value, nameof(August));
                if (HasErrors)
                {
                    return;
                }
                BindingPrecipitationData.August = value;
                RaisePropertyChanged(nameof(August));
            }
        }
        public double September
        {
            get => BindingPrecipitationData.September;
            set
            {
                ValidateValue(value, nameof(September));
                if (HasErrors)
                {
                    return;
                }
                BindingPrecipitationData.September = value;
                RaisePropertyChanged(nameof(September));
            }
        }
        public double October
        {
            get => BindingPrecipitationData.October;
            set
            {
                ValidateValue(value, nameof(October));
                if (HasErrors)
                {
                    return;
                }
                BindingPrecipitationData.October = value;
                RaisePropertyChanged(nameof(October));
            }
        }
        public double November
        {
            get => BindingPrecipitationData.November;
            set
            {
                ValidateValue(value, nameof(November));
                if (HasErrors)
                {
                    return;
                }
                BindingPrecipitationData.November = value;
                RaisePropertyChanged(nameof(November));
            }
        }
        public double December
        {
            get => BindingPrecipitationData.December;
            set
            {
                ValidateValue(value, nameof(December));
                if (HasErrors)
                {
                    return;
                }
                BindingPrecipitationData.December = value;
                RaisePropertyChanged(nameof(December));
            }
        }
        #endregion
        #region Methods
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
        public void ManageData()
        {
            BindingPrecipitationData = ActiveFarm.ClimateData.PrecipitationData;
            BindingPrecipitationData.PropertyChanged -= OnDataPropertyChanged;
            BindingPrecipitationData.PropertyChanged += OnDataPropertyChanged;
        }
        private void OnDataPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is PrecipitationData)
            {
                ActiveFarm.ClimateData.PrecipitationData = BindingPrecipitationData;
            }
        }
        #endregion
    }
}
