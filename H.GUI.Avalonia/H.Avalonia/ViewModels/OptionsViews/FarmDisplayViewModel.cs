using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H.Core.Services.StorageService;
using Mapsui.Extensions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class FarmDisplayViewModel : ViewModelBase
    {
        #region Fields
        private string _coordinates;
        private string _farmName;
        private string _farmComments;
        private double _growingSeasonPrecipitation;
        private double _growingSeasonEvapotranspiration;
        #endregion

        #region Constructors
        public FarmDisplayViewModel(IStorageService storageService) : base(storageService)
        {
            ActiveFarm = base.StorageService.GetActiveFarm();
        }
        #endregion
        #region Properties
        public string Coordinates
        {
            get => _coordinates;
            set => SetProperty(ref _coordinates, value);
        }
        public string FarmComments
        {
            get => _farmComments;
            set
            {
                if (SetProperty(ref _farmComments, value))
                {
                    ValidateFarmComments();
                    if (HasErrors)
                    {
                        return;
                    }
                    if (ActiveFarm.Comments != value)
                    {
                        ActiveFarm.Comments = value;
                    }

                }
            }
        }
        public string FarmName
        {
            get => _farmName;
            set
            {
                if (SetProperty(ref _farmName, value))
                {
                    ValidateFarmName();
                    if (HasErrors)
                    {
                        return;
                    }
                    if (ActiveFarm.Name != value)
                    {
                        ActiveFarm.Name = value;
                    }

                }
            }
        }
        public double GrowingSeasonPrecipitation
        {
            get => _growingSeasonPrecipitation;
            set
            {
                if (SetProperty(ref _growingSeasonPrecipitation, value))
                {
                    ValidatePrecipitation();
                    if (HasErrors)
                    {
                        return;
                    }
                    if (ActiveFarm.ClimateData.PrecipitationData.GrowingSeasonPrecipitation != value)
                    {
                        ActiveFarm.ClimateData.PrecipitationData.GrowingSeasonPrecipitation = value;
                    }

                }
            }
        }
        public double GrowingSeasonEvapotranspiration
        {
            get => _growingSeasonEvapotranspiration;
            set
            {
                if (SetProperty(ref _growingSeasonEvapotranspiration, value))
                {
                    ValidateEvapotranspiration();
                    if (HasErrors)
                    {
                        return;
                    }
                    if (ActiveFarm.ClimateData.EvapotranspirationData.GrowingSeasonEvapotranspiration != value)
                    {
                        ActiveFarm.ClimateData.EvapotranspirationData.GrowingSeasonEvapotranspiration = value;
                    }

                }
            }
        }
        #endregion
        #region Private Methods
        private void ValidateFarmName()
        {
            if (string.IsNullOrWhiteSpace(FarmName))
            {
                AddError(nameof(FarmName), "Farm name is required.");
            }
            else
            {
                RemoveError(nameof(FarmName));
            }

        }
        private void ValidateFarmComments()
        {
            if (string.IsNullOrWhiteSpace(FarmComments))
            {
                AddError(nameof(FarmComments), "Farm comments are required.");
            }
            else
            {
                RemoveError(nameof(FarmComments));
            }
        }
        private void ValidatePrecipitation()
        {
            if (GrowingSeasonPrecipitation.IsNanOrInfOrZero())
            {
                AddError(nameof(GrowingSeasonPrecipitation), "Growing season precipitation must be greater than 0.");
            }
            else
            {
                RemoveError(nameof(GrowingSeasonPrecipitation));
            }
        }
        private void ValidateEvapotranspiration()
        {
            if (GrowingSeasonEvapotranspiration.IsNanOrInfOrZero())
            {
                AddError(nameof(GrowingSeasonEvapotranspiration), "Growing season evapotranspiration must be greater than 0.");
            }
            else
            {
                RemoveError(nameof(GrowingSeasonEvapotranspiration));
            }
        }
        #endregion
    }
}
