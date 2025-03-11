using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapsui.Extensions;
using H.Core.Services.StorageService;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class SoilN2OBreakdownDisplayViewModel : ViewModelBase
    {
        #region Fields
        private double _january;
        private double _february;
        private double _march;
        private double _april;
        private double _may;
        public double _june;
        private double _july;
        private double _august;
        private double _september;
        private double _october;
        private double _november;
        private double _december;
        #endregion
        #region Constructors
        public SoilN2OBreakdownDisplayViewModel(IStorageService storageService) : base(storageService)
        {
            ActiveFarm = base.StorageService.GetActiveFarm();
        }
        #endregion
        #region Properties
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
                    if (ActiveFarm.AnnualSoilN2OBreakdown.January != value)
                    {
                        ActiveFarm.AnnualSoilN2OBreakdown.January = value;
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
                    if (ActiveFarm.AnnualSoilN2OBreakdown.February != value)
                    {
                        ActiveFarm.AnnualSoilN2OBreakdown.February = value;
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
                    if (ActiveFarm.AnnualSoilN2OBreakdown.March != value)
                    {
                        ActiveFarm.AnnualSoilN2OBreakdown.March = value;
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
                    if (ActiveFarm.AnnualSoilN2OBreakdown.April != value)
                    {
                        ActiveFarm.AnnualSoilN2OBreakdown.April = value;
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
                    if (ActiveFarm.AnnualSoilN2OBreakdown.May != value)
                    {
                        ActiveFarm.AnnualSoilN2OBreakdown.May = value;
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
                    if (ActiveFarm.AnnualSoilN2OBreakdown.June != value)
                    {
                        ActiveFarm.AnnualSoilN2OBreakdown.June = value;
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
                    if (ActiveFarm.AnnualSoilN2OBreakdown.July != value)
                    {
                        ActiveFarm.AnnualSoilN2OBreakdown.July = value;
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
                    if (ActiveFarm.AnnualSoilN2OBreakdown.August != value)
                    {
                        ActiveFarm.AnnualSoilN2OBreakdown.August = value;
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
                    if (ActiveFarm.AnnualSoilN2OBreakdown.September != value)
                    {
                        ActiveFarm.AnnualSoilN2OBreakdown.September = value;
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
                    if (ActiveFarm.AnnualSoilN2OBreakdown.October != value)
                    {
                        ActiveFarm.AnnualSoilN2OBreakdown.October = value;
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
                    if (ActiveFarm.AnnualSoilN2OBreakdown.November != value)
                    {
                        ActiveFarm.AnnualSoilN2OBreakdown.November = value;
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
                    if (ActiveFarm.AnnualSoilN2OBreakdown.December != value)
                    {
                        ActiveFarm.AnnualSoilN2OBreakdown.December = value;
                    }
                }
                
            }
        }
        #endregion
        #region Methods
        private void ValidateJanuary()
        {
            if (January < 0.00 || January > 100.00)
            {
                AddError(nameof(January), "Percentage must be between 0 and 100");
            }
            else
            {
                RemoveError(nameof(January));
            }
        }
        private void ValidateFebruary()
        {
            if (February < 0.00 || February > 100.00)
            {
                AddError(nameof(February), "Percentage must be between 0 and 100");
            }
            else
            {
                RemoveError(nameof(February));
            }
        }
        private void ValidateMarch()
        {
            if (March < 0.00 || March > 100.00)
            {
                AddError(nameof(March), "Percentage must be between 0 and 100");
            }
            else
            {
                RemoveError(nameof(March));
            }
        }
        private void ValidateApril()
        {
            if (April < 0.00 || April > 100.00)
            {
                AddError(nameof(April), "Percentage must be between 0 and 100");
            }
            else
            {
                RemoveError(nameof(April));
            }
        }
        private void ValidateMay()
        {
            if (May < 0.00 || May > 100.00)
            {
                AddError(nameof(May), "Percentage must be between 0 and 100");
            }
            else
            {
                RemoveError(nameof(May));
            }
        }
        private void ValidateJune()
        {
            if (June < 0.00 || June > 100.00)
            {
                AddError(nameof(June), "Percentage must be between 0 and 100");
            }
            else
            {
                RemoveError(nameof(June));
            }
        }
        private void ValidateJuly()
        {
            if (July < 0.00 || July > 100.00)
            {
                AddError(nameof(July), "Percentage must be between 0 and 100");
            }
            else
            {
                RemoveError(nameof(July));
            }
        }
        private void ValidateAugust()
        {
            if (August < 0.00 || August > 100.00)
            {
                AddError(nameof(August), "Percentage must be between 0 and 100");
            }
            else
            {
                RemoveError(nameof(August));
            }
        }
        private void ValidateSeptember()
        {
            if (September < 0.00 || September > 100.00)
            {
                AddError(nameof(September), "Percentage must be between 0 and 100");
            }
            else
            {
                RemoveError(nameof(September));
            }
        }
        private void ValidateOctober()
        {
            if (October < 0.00 || October > 100.00)
            {
                AddError(nameof(October), "Percentage must be between 0 and 100");
            }
            else
            {
                RemoveError(nameof(October));
            }
        }
        private void ValidateNovember()
        {
            if (November < 0.00 || November > 100.00)
            {
                AddError(nameof(November), "Percentage must be between 0 and 100");
            }
            else
            {
                RemoveError(nameof(November));
            }
        }
        private void ValidateDecember()
        {
            if (December < 0.00 || December > 100.00)
            {
                AddError(nameof(December), "Percentage must be between 0 and 100");
            }
            else
            {
                RemoveError(nameof(December));
            }
        }
        #endregion
    }
}
