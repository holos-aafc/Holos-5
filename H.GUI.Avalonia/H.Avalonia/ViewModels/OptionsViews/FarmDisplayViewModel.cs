using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExCSS;
using H.Core.Services.StorageService;
using Mapsui.Extensions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class FarmDisplayViewModel : ViewModelBase
    {
        #region Fields
        private string _coordinates;
        #endregion

        #region Constructors
        public FarmDisplayViewModel(IStorageService storageService) : base(storageService)
        {
            Coordinates = $"{ActiveFarm.Latitude}, {ActiveFarm.Longitude}";
        }
        #endregion

        #region Properties
        ///Wrapper properties for validating and setting values
        public string Coordinates
        {
            get => _coordinates;
            set => SetProperty(ref _coordinates, value);
        }
        public string FarmComments
        {
            get => ActiveFarm.Comments;
            set
            {
                ValidateString(value, nameof(FarmComments));
                if (HasErrors)
                    {
                        return;
                    }
                     ActiveFarm.Comments = value;
                RaisePropertyChanged(nameof(FarmComments));

            }
        }
        public string FarmName
        {
            get => ActiveFarm.Name;
            set
            {
                ValidateString(value, nameof(FarmName));
                    if (HasErrors)
                    {
                        return;
                    }
                    ActiveFarm.Name = value;
                    RaisePropertyChanged(nameof(FarmName));
            }
        }
        public double GrowingSeasonPrecipitation
        {
            get => ActiveFarm.ClimateData.PrecipitationData.GrowingSeasonPrecipitation;
            set
            {
                ValidateNonNegative(value, nameof(GrowingSeasonPrecipitation));
                    if (HasErrors)
                    {
                        return;
                    }
                    ActiveFarm.ClimateData.PrecipitationData.GrowingSeasonPrecipitation = value;
                    RaisePropertyChanged(nameof(GrowingSeasonPrecipitation));
            }
        }
        public double GrowingSeasonEvapotranspiration
        {
            get => ActiveFarm.ClimateData.EvapotranspirationData.GrowingSeasonEvapotranspiration;
            set
            {
                ValidateNonNegative(value, nameof(GrowingSeasonEvapotranspiration));
                    if (HasErrors)
                    {
                        return;
                    }
                    ActiveFarm.ClimateData.EvapotranspirationData.GrowingSeasonEvapotranspiration = value;
                    RaisePropertyChanged(nameof(GrowingSeasonEvapotranspiration));
            }
        }
        #endregion

        #region Private Methods
        ///Validation methods for properties
        private void ValidateString(string value, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                AddError(propertyName, H.Core.Properties.Resources.ErrorNameCannotBeEmpty);
            }
            else
            {
                RemoveError(propertyName);
            }

        }
        private void ValidateNonNegative(double value, string propertyName)
        {
            if (value.IsNanOrInfOrZero())
            {
                AddError(propertyName, H.Core.Properties.Resources.ErrorMustBeGreaterThan0);
            }
            else
            {
                RemoveError(propertyName);
            }
        }
        #endregion
    }
}
