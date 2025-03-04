using H.Core.Services.StorageService;
using Mapsui.Extensions;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class OptionFarmViewModel : ViewModelBase
    {
        private string _coordinates;
        private string _farmName;
        private string _farmComments;
        private double _growingSeasonPrecipitation;
        private double _growingSeasonEvapotranspiration;
        private H.Core.Models.Farm _activeFarm;
        public OptionFarmViewModel() { }
        public OptionFarmViewModel(IRegionManager regionManager, IStorageService storageService) : base(regionManager, storageService)
        {
        }

        public H.Core.Models.Farm ActiveFarm
        {
            get => _activeFarm;
            set => SetProperty(ref _activeFarm, value); 
        }
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
                        // Optionally notify the user that there are validation errors
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
                        // Optionally notify the user that there are validation errors
                        return;
                    }
                    if(ActiveFarm.Name != value)
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
                        // Optionally notify the user that there are validation errors
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
                        // Optionally notify the user that there are validation errors
                        return;
                    }
                    if (ActiveFarm.ClimateData.EvapotranspirationData.GrowingSeasonEvapotranspiration != value)
                    {
                        ActiveFarm.ClimateData.EvapotranspirationData.GrowingSeasonEvapotranspiration = value;
                    }

                }
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            ActiveFarm = base.StorageService.GetActiveFarm();
            FarmName = ActiveFarm.Name;
            FarmComments = ActiveFarm.Comments;
            Coordinates = $"{ActiveFarm.Latitude}, {ActiveFarm.Longitude}";
            GrowingSeasonPrecipitation = ActiveFarm.ClimateData.PrecipitationData.GrowingSeasonPrecipitation;
            GrowingSeasonEvapotranspiration = ActiveFarm.ClimateData.EvapotranspirationData.GrowingSeasonEvapotranspiration;
            base.OnNavigatedTo(navigationContext);
        }
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
    }
}
