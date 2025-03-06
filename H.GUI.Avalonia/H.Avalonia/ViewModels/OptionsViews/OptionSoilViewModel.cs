using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H.Core.Converters;
using H.Core.Enumerations;
using H.Core.Services.StorageService;
using Mapsui.Extensions;
using NetTopologySuite.Geometries;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class OptionSoilViewModel : ViewModelBase
    {
        private SoilTexture _selectedSoilTexture;
        private double _topLayerThickness;
        private double _bulkDensity;
        private double _proportionOfClayInSoil;
        private double _proportionOfSandInSoil;
        private double _proportionOfSoilOrganicCarbon;
        private double _soilPh;
        private double _soilCec;
        private int _carbonModellingEquilibriumYear;
        public OptionSoilViewModel() { }
        public OptionSoilViewModel(IRegionManager regionManager, IStorageService storageService) : base(regionManager, storageService)
        {
            SoilTextures = new ObservableCollection<SoilTexture>();
            ActiveFarm = base.StorageService.GetActiveFarm();
        }

        public SoilTexture SelectedSoilTexture
        {
            get => _selectedSoilTexture;
            set
            {
                if (SetProperty(ref _selectedSoilTexture, value))
                {
                    ActiveFarm.DefaultSoilData.SoilTexture = value;
                }
            }
        }
        public double TopLayerThickness
        {
            get => _topLayerThickness;
            set
            {
                if (SetProperty(ref _topLayerThickness, value))
                {
                    ValidateTopLayerThickness();
                    if (HasErrors)
                    {
                        // Optionally notify the user that there are validation errors
                        return;
                    }
                    if (ActiveFarm.DefaultSoilData.TopLayerThickness != value)
                    {
                        ActiveFarm.DefaultSoilData.TopLayerThickness = value;
                    }
                }
            }
        }
        public double BulkDensity
        {
            get => _bulkDensity;
            set
            {
                if (SetProperty(ref _bulkDensity, value))
                {
                    ValidateBulkDensity();
                    if (HasErrors)
                    {
                        // Optionally notify the user that there are validation errors
                        return;
                    }
                    if (ActiveFarm.DefaultSoilData.BulkDensity != value)
                    {
                        ActiveFarm.DefaultSoilData.BulkDensity = value;
                    }
                }
            }
        }
        public double ProportionOfClayInSoil
        {
            get => _proportionOfClayInSoil;
            set
            {
                if (SetProperty(ref _proportionOfClayInSoil, value))
                {
                    ValidateProportionOfClayInSoil();
                    if (HasErrors) {
                        // Optionally notify the user that there are validation errors
                        return;
                    }
                    if (ActiveFarm.DefaultSoilData.ProportionOfClayInSoil != value)
                    {
                        ActiveFarm.DefaultSoilData.ProportionOfClayInSoil = value;
                    }
                }
            }
        }
        public double ProportionOfSandInSoil
        {
            get => _proportionOfSandInSoil;
            set
            {
                if (SetProperty(ref _proportionOfSandInSoil, value))
                {
                    ValidateProportionOfSandInSoil();
                    if (HasErrors)
                    {
                        // Optionally notify the user that there are validation errors
                        return;
                    }
                    if (ActiveFarm.DefaultSoilData.ProportionOfSandInSoil != value)
                    {
                        ActiveFarm.DefaultSoilData.ProportionOfSandInSoil = value;
                    }
                }
            }
        }
        public double SoilPh
        {
            get => _soilPh;
            set
            {
                if (SetProperty(ref _soilPh, value))
                {
                    ValidateSoilPh();
                    if (HasErrors)
                    {
                        // Optionally notify the user that there are validation errors
                        return;
                    }
                    if (ActiveFarm.DefaultSoilData.SoilPh != value)
                    {
                        ActiveFarm.DefaultSoilData.SoilPh = value;
                    }
                }
            }
        }
        private double ProportionOfSoilOrganicCarbon
        {
            get => _proportionOfSoilOrganicCarbon;
            set
            {
                if (SetProperty(ref _proportionOfSoilOrganicCarbon, value))
                {
                    ValidateProportionOfSoilOrganicCarbon();
                    if (HasErrors)
                    {
                        // Optionally notify the user that there are validation errors
                        return;
                    }
                    if (ActiveFarm.DefaultSoilData.ProportionOfSoilOrganicCarbon != value)
                    {
                        ActiveFarm.DefaultSoilData.ProportionOfSoilOrganicCarbon = value;
                    }
                }
            }
        }
        public double SoilCec
        {
            get => _soilCec;
            set
            {
                if (SetProperty(ref _soilCec, value))
                {
                    ValidateSoilCec();
                    if (HasErrors)
                    {
                        // Optionally notify the user that there are validation errors
                        return;
                    }
                    if (ActiveFarm.DefaultSoilData.SoilCec != value)
                    {
                        ActiveFarm.DefaultSoilData.SoilCec = value;
                    }
                }
            }
        }
        public int CarbonModellingEquilibriumYear
        {
            get => _carbonModellingEquilibriumYear;
            set
            {
                if (SetProperty(ref _carbonModellingEquilibriumYear, value))
                {
                    ValidateCarbonModellingEquilibriumYear();
                    if (HasErrors)
                    {
                        // Optionally notify the user that there are validation errors
                        return;
                    }
                    if (ActiveFarm.CarbonModellingEquilibriumYear != value)
                    {
                        ActiveFarm.CarbonModellingEquilibriumYear = value;
                    }
                }
            }
        }
        public ObservableCollection<SoilTexture> SoilTextures { get; set; }
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            var soilTextures = SoilTexture.GetValues(typeof(SoilTexture));
            foreach (SoilTexture soilTexture in soilTextures)
            {
                if(!SoilTextures.Contains(soilTexture))
                {
                    SoilTextures.Add(soilTexture);
                } 
            }
            SelectedSoilTexture = ActiveFarm.DefaultSoilData.SoilTexture;
            TopLayerThickness = ActiveFarm.DefaultSoilData.TopLayerThickness;
            BulkDensity = ActiveFarm.DefaultSoilData.BulkDensity;
            ProportionOfClayInSoil = ActiveFarm.DefaultSoilData.ProportionOfClayInSoil;
            ProportionOfSandInSoil = ActiveFarm.DefaultSoilData.ProportionOfSandInSoil;
            ProportionOfSoilOrganicCarbon = ActiveFarm.DefaultSoilData.ProportionOfSoilOrganicCarbon;
            SoilPh = ActiveFarm.DefaultSoilData.SoilPh;
            SoilCec = ActiveFarm.DefaultSoilData.SoilCec;
            CarbonModellingEquilibriumYear = ActiveFarm.CarbonModellingEquilibriumYear;
            base.OnNavigatedTo(navigationContext);
        }
        private void ValidateTopLayerThickness()
        {
            if (TopLayerThickness.IsNanOrInfOrZero())
            {
                AddError(nameof(TopLayerThickness), "Top Layer Thickness must be greater than 0.");
            }
            else
            {
                RemoveError(nameof(TopLayerThickness));
            }
        }
        private void ValidateBulkDensity()
        {
            if (BulkDensity.IsNanOrInfOrZero())
            {
                AddError(nameof(BulkDensity), "Bulk Density must be greater than 0.");
            }
            else
            {
                RemoveError(nameof(BulkDensity));
            }
        }
        private void ValidateProportionOfClayInSoil()
        {
            if (ProportionOfClayInSoil.IsNanOrInfOrZero())
            {
                AddError(nameof(ProportionOfClayInSoil), "Proportion of Clay in Soil must be greater than 0.");
            }
            else
            {
                RemoveError(nameof(ProportionOfClayInSoil));
            }
        }
        private void ValidateProportionOfSandInSoil()
        {
            if (ProportionOfSandInSoil.IsNanOrInfOrZero())
            {
                AddError(nameof(ProportionOfSandInSoil), "Proportion of Sand in Soil must be greater than 0.");
            }
            else
            {
                RemoveError(nameof(ProportionOfSandInSoil));
            }
        }
        private void ValidateProportionOfSoilOrganicCarbon()
        {
            if (ProportionOfSoilOrganicCarbon.IsNanOrInfOrZero())
            {
                AddError(nameof(ProportionOfSoilOrganicCarbon), "Proportion of Soil Organic Carbon must be greater than 0.");
            }
            else
            {
                RemoveError(nameof(ProportionOfSoilOrganicCarbon));
            }
        }
        private void ValidateSoilPh()
        {
            if (SoilPh.IsNanOrInfOrZero())
            {
                AddError(nameof(SoilPh), "Soil pH must be greater than 0.");
            }
            else
            {
                RemoveError(nameof(SoilPh));
            }
        }
        private void ValidateSoilCec()
        {
            if (SoilCec.IsNanOrInfOrZero())
            {
                AddError(nameof(SoilCec), "Soil CEC must be greater than 0.");
            }
            else
            {
                RemoveError(nameof(SoilCec));
            }
        }
        private void ValidateCarbonModellingEquilibriumYear()
        {
            int currentYear = DateTime.Today.Year;
            if (CarbonModellingEquilibriumYear > currentYear || CarbonModellingEquilibriumYear <= 0)
            {
                AddError(nameof(CarbonModellingEquilibriumYear), "Must be a valid year.");
            }
            else
            {
                RemoveError(nameof(CarbonModellingEquilibriumYear));
            }
        }
    }
}
