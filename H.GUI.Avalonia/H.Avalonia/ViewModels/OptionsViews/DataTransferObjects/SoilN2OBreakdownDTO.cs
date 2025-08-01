using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapsui.Extensions;
using H.Core.Services.StorageService;
using ExCSS;

namespace H.Avalonia.ViewModels.OptionsViews.DataTransferObjects
{
    public class SoilN2OBreakdownDTO : ViewModelBase
    {
 
        #region Constructors
        public SoilN2OBreakdownDTO(IStorageService storageService) : base(storageService)
        {          
        }
        #endregion

        #region Properties
        ///Wrapper properties for validating and setting values
        public double January
        {
            get => ActiveFarm.AnnualSoilN2OBreakdown.January;
            set
            {
                ValidatePercentage(value, nameof(January));
                if (HasErrors)
                    {
                        return;
                    }
                     ActiveFarm.AnnualSoilN2OBreakdown.January = value;
                    RaisePropertyChanged(nameof(January));
            }
        }
        public double February
        {
            get => ActiveFarm.AnnualSoilN2OBreakdown.February;
            set
            {
                ValidatePercentage(value, nameof(February));
                if (HasErrors)
                    {
                        return;
                    }
                    ActiveFarm.AnnualSoilN2OBreakdown.February = value;
                    RaisePropertyChanged(nameof(February));
            }
        }
        public double March
        {
            get => ActiveFarm.AnnualSoilN2OBreakdown.March;
            set
            {
                ValidatePercentage(value, nameof(March));
                if (HasErrors)
                    {
                        return;
                    }
                    ActiveFarm.AnnualSoilN2OBreakdown.March = value;
                    RaisePropertyChanged(nameof(March));
            }
        }
        public double April
        {
            get => ActiveFarm.AnnualSoilN2OBreakdown.April;
            set
            {

                ValidatePercentage(value, nameof(April));
                if (HasErrors)
                    {
                        return;
                    }
                    ActiveFarm.AnnualSoilN2OBreakdown.April = value;
                   RaisePropertyChanged(nameof(April));
            }
        }
        public double May
        {
            get => ActiveFarm.AnnualSoilN2OBreakdown.May;
            set
            {
                ValidatePercentage(value, nameof(May));
                if (HasErrors)
                    {
                        return;
                    }
                    ActiveFarm.AnnualSoilN2OBreakdown.May = value;
                    RaisePropertyChanged(nameof(May));
            }
        }
        public double June
        {
            get => ActiveFarm.AnnualSoilN2OBreakdown.June;
            set
            {
                ValidatePercentage(value, nameof(June));
                if (HasErrors)
                    {
                        return;
                    }
                   ActiveFarm.AnnualSoilN2OBreakdown.June = value;
                   RaisePropertyChanged(nameof(June));
            }
        }
        public double July
        {
            get => ActiveFarm.AnnualSoilN2OBreakdown.July;
            set
            {
                ValidatePercentage(value, nameof(July));
                if (HasErrors)
                    {
                        return;
                    }
                    ActiveFarm.AnnualSoilN2OBreakdown.July = value;
            }
        }
        public double August
        {
            get => ActiveFarm.AnnualSoilN2OBreakdown.August;
            set
            {
                ValidatePercentage(value, nameof(August));
                if (HasErrors)
                    {
                        return;
                    }
                    ActiveFarm.AnnualSoilN2OBreakdown.August = value;
                    RaisePropertyChanged(nameof(August));

            }
        }
        public double September
        {
            get => ActiveFarm.AnnualSoilN2OBreakdown.September;
            set
            {
                ValidatePercentage(value, nameof(September));
                if (HasErrors)
                    {
                        return;
                    }
                    ActiveFarm.AnnualSoilN2OBreakdown.September = value;
                    RaisePropertyChanged(nameof(September));

            }
        }
        public double October
        {
            get => ActiveFarm.AnnualSoilN2OBreakdown.October;
            set
            {
                ValidatePercentage(value, nameof(October));
                if (HasErrors)
                    {
                        return;
                    }
                     ActiveFarm.AnnualSoilN2OBreakdown.October = value;
                    RaisePropertyChanged(nameof(October));

            }
        }
        public double November
        {
            get => ActiveFarm.AnnualSoilN2OBreakdown.November;
            set
            {
                ValidatePercentage(value, nameof(November));
                if (HasErrors)
                    {
                        return;
                    }
                    ActiveFarm.AnnualSoilN2OBreakdown.November = value;
                    RaisePropertyChanged(nameof(November));
            }
        }
        public double December
        {
            get => ActiveFarm.AnnualSoilN2OBreakdown.December;
            set
            {
                ValidatePercentage(value, nameof(December));
                    if (HasErrors)
                    {
                        return;
                    }
                    ActiveFarm.AnnualSoilN2OBreakdown.December = value;
                    RaisePropertyChanged(nameof(December));

            }
        }
        #endregion

        #region Methods
        ///Validation methods
        private void ValidatePercentage(double value, string propertyName)
        {
            if (value < 0.00 || value > 100.00)
            {
                AddError(propertyName, H.Core.Properties.Resources.ErrorMustBeBetween0And100);
            }
            else
            {
                RemoveError(propertyName);
            }
        }
        #endregion
    }
}
