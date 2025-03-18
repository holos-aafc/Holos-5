using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H.Core.Enumerations;
using H.Core.Services.StorageService;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class PrecipitationDisplayViewModel : ViewModelBase
    {
        #region Fields
        private double _january;
        private double _february;
        private double _march;
        private double _april;
        private double _may;
        private double _june;
        private double _july;
        private double _august;
        private double _september;
        private double _october;
        private double _november;
        private double _december;
        #endregion
        #region Constructors
        public PrecipitationDisplayViewModel(IStorageService storageService) : base(storageService)
        {
            ActiveFarm = StorageService.GetActiveFarm();
            PrecipitationSeriesValues = new ObservableCollection<double>();
            CreatePrecipitationSeries();
        }
        #endregion
        #region Properties
        public ObservableCollection<double> PrecipitationSeriesValues { get; set; }
        public double January
        {
            get => _january;
            set
            {
                if (SetProperty(ref _january, value))
                {
                    ValidateJanuary();
                    if (HasErrors)
                    {
                        return;
                    }
                    if (ActiveFarm.ClimateData.PrecipitationData.January != value)
                    {
                        ActiveFarm.ClimateData.PrecipitationData.January = value;
                        PrecipitationSeriesValues[0] = value;
                    }
                }
            }
        }
        public double February
        {
            get => _february;
            set
            {
                if (SetProperty(ref _february, value))
                {
                    ValidateFebruary();
                    if (HasErrors)
                    {
                        return;
                    }
                    if (ActiveFarm.ClimateData.PrecipitationData.February != value)
                    {
                        ActiveFarm.ClimateData.PrecipitationData.February = value;
                        PrecipitationSeriesValues[1] = value;
                    }
                }
            }
        }
        public double March
        {
            get => _march;
            set
            {
                if (SetProperty(ref _march, value))
                {
                    ValidateMarch();
                    if (HasErrors)
                    {
                        return;
                    }
                    if (ActiveFarm.ClimateData.PrecipitationData.March != value)
                    {
                        ActiveFarm.ClimateData.PrecipitationData.March = value;
                        PrecipitationSeriesValues[2] = value;
                    }
                }
            }
        }
        public double April
        {
            get => _april;
            set
            {
                if (SetProperty(ref _april, value))
                {
                    ValidateApril();
                    if (HasErrors)
                    {
                        return;
                    }
                    if (ActiveFarm.ClimateData.PrecipitationData.April != value)
                    {
                        ActiveFarm.ClimateData.PrecipitationData.April = value;
                        PrecipitationSeriesValues[3] = value;
                    }
                }
            }
        }
        public double May
        {
            get => _may;
            set
            {
                if (SetProperty(ref _may, value))
                {
                    ValidateMay();
                    if (HasErrors)
                    {
                        return;
                    }
                    if (ActiveFarm.ClimateData.PrecipitationData.May != value)
                    {
                        ActiveFarm.ClimateData.PrecipitationData.May = value;
                        PrecipitationSeriesValues[4] = value;
                    }
                }
            }
        }
        public double June
        {
            get => _june;
            set
            {
                if (!SetProperty(ref _june, value))
                {
                    ValidateJune();
                    if (HasErrors)
                    {
                        return;
                    }
                    if (ActiveFarm.ClimateData.PrecipitationData.June != value)
                    {
                        ActiveFarm.ClimateData.PrecipitationData.June = value;
                        PrecipitationSeriesValues[5] = value;
                    }
                }
            }
        }
        public double July
        {
            get => _july;
            set
            {
                if (SetProperty(ref _july, value))
                {
                    ValidateJuly();
                    if (HasErrors)
                    {
                        return;
                    }
                    if (ActiveFarm.ClimateData.PrecipitationData.July != value)
                    {
                        ActiveFarm.ClimateData.PrecipitationData.July = value;
                        PrecipitationSeriesValues[6] = value;
                    }
                }

            }
        }
        public double August
        {
            get => _august;
            set
            {
                if (SetProperty(ref _august, value))
                {
                    ValidateAugust();
                    if (HasErrors)
                    {
                        return;
                    }
                    if (ActiveFarm.ClimateData.PrecipitationData.August != value)
                    {
                        ActiveFarm.ClimateData.PrecipitationData.August = value;
                        PrecipitationSeriesValues[7] = value;
                    }
                }

            }
        }
        public double September
        {
            get => _september;
            set
            {
                if (SetProperty(ref _september, value))
                {
                    ValidateSeptember();
                    if (HasErrors)
                    {
                        return;
                    }
                    if (ActiveFarm.ClimateData.PrecipitationData.September != value)
                    {
                        ActiveFarm.ClimateData.PrecipitationData.September = value;
                        PrecipitationSeriesValues[8] = value;
                    }
                }

            }
        }
        public double October
        {
            get => _october;
            set
            {
                if (SetProperty(ref _october, value))
                {
                    ValidateOctober();
                    if (HasErrors)
                    {
                        return;
                    }
                    if (ActiveFarm.ClimateData.PrecipitationData.October != value)
                    {
                        ActiveFarm.ClimateData.PrecipitationData.October = value;
                        PrecipitationSeriesValues[9] = value;
                    }
                }

            }
        }
        public double November
        {
            get => _november;
            set
            {
                if (SetProperty(ref _november, value))
                {
                    ValidateNovember();
                    if (HasErrors)
                    {
                        return;
                    }
                    if (ActiveFarm.ClimateData.PrecipitationData.November != value)
                    {
                        ActiveFarm.ClimateData.PrecipitationData.November = value;
                        PrecipitationSeriesValues[10] = value;
                    }
                }
            }
        }
        public double December
        {
            get => _december;
            set
            {
                if (SetProperty(ref _december, value))
                {
                    ValidateDecember();
                    if (HasErrors)
                    {
                        return;
                    }
                    if (ActiveFarm.ClimateData.PrecipitationData.December != value)
                    {
                        ActiveFarm.ClimateData.PrecipitationData.December = value;
                        PrecipitationSeriesValues[11] = value;
                    }
                }

            }
        }
        #endregion
        #region Methods
        private void CreatePrecipitationSeries()
        {
            var months = Enum.GetValues(typeof(H.Core.Enumerations.Months));
            PrecipitationSeriesValues.Clear();
            foreach (Months month in months)
            {
                PrecipitationSeriesValues.Add(ActiveFarm.ClimateData.PrecipitationData.GetValueByMonth(month));
            }
        }
        private void ValidateJanuary()
        {
            if (January < 0.00 || January > 100.00)
            {
                AddError(nameof(January), "Value cannot be below zero");
            }
            else
            {
                RemoveError(nameof(January));
            }
        }
        private void ValidateFebruary()
        { 
            if (February < 0.00)
            {
                AddError(nameof(February), "Value cannot be below zero");
            }
            else
            {
                RemoveError(nameof(February));
            }
        }
        private void ValidateMarch()
        {
            if (March < 0.00)
            {
                AddError(nameof(March), "Value cannot be below zero");
            }
            else
            {
                RemoveError(nameof(March));
            }
        }
        private void ValidateApril()
        { 
            if (April < 0.00)
            {
                AddError(nameof(April), "Value cannot be below zero");
            }
            else
            {
                RemoveError(nameof(April));
            }
        }
        private void ValidateMay()
        {
            if (May < 0.00)
            {
                AddError(nameof(May), "Value cannot be below zero");
            }
            else
            {
                RemoveError(nameof(May));
            }
        }
        private void ValidateJune()
        {
            if (June < 0.00)
            {
                AddError(nameof(June), "Value cannot be below zero");
            }
            else
            {
                RemoveError(nameof(June));
            }
        }
        private void ValidateJuly()
        {
            if (July < 0.00)
            {
                AddError(nameof(July), "Value cannot be below zero");
            }
            else
            {
                RemoveError(nameof(July));
            }
        }
        private void ValidateAugust()
        {
            if (August < 0.00)
            {
                AddError(nameof(August), "Value cannot be below zero");
            }
            else
            {
                RemoveError(nameof(August));
            }
        }
        private void ValidateSeptember()
        {
            if (September < 0.00)
            {
                AddError(nameof(September), "Value cannot be below zero");
            }
            else
            {
                RemoveError(nameof(September));
            }
        }
        private void ValidateOctober()
        {
            if (October < 0.00)
            {
                AddError(nameof(October), "Value cannot be below zero");
            }
            else
            {
                RemoveError(nameof(October));
            }
        }
        private void ValidateNovember()
        {
            if (November < 0.00)
            {
                AddError(nameof(November), "Value cannot be below zero");
            }
            else
            {
                RemoveError(nameof(November));
            }
        }
        private void ValidateDecember()
        {
            if (December < 0.00)
            {
                AddError(nameof(December), "Value cannot be below zero");
            }
            else
            {
                RemoveError(nameof(December));
            }
        }
        #endregion
    }
}
