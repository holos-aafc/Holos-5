using System;
using H.Core.Enumerations;
using H.Core.Providers.Precipitation;
using H.Core.Providers.Soil;
using H.Core.Services.StorageService;
using Mapsui.Extensions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class SoilDisplayViewModel : ViewModelBase
    {
        #region Fields
        private SoilTexture _selectedSoilTexture;        
        private int _carbonModellingEquilibriumYear; 
        private SoilData _bindingSoilData = new SoilData();
        #endregion
        #region Constructors
        public SoilDisplayViewModel(IStorageService storageService) : base(storageService) 
        {
            ManageData();
        }
        #endregion
        #region Properties
        public SoilData BindingSoilData
        {
            get => _bindingSoilData;
            set => SetProperty(ref _bindingSoilData, value);
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
        public double BulkDensity
        {
            get => BindingSoilData.BulkDensity;
            set
            {
               ValidateDouble(value, nameof(BulkDensity));
                if (HasErrors)
                {
                    return;
                }
                BindingSoilData.BulkDensity = value;
            }
        }
        public double TopLayerThickness
        {
            get => BindingSoilData.TopLayerThickness;
            set
            {
                ValidateDouble(value, nameof(TopLayerThickness));
                if (HasErrors)
                {
                    return;
                }
                BindingSoilData.TopLayerThickness = value;
            }
        }
        public double ProportionOfClayInSoil
        {
            get => BindingSoilData.ProportionOfClayInSoil;
            set
            {
                ValidateDouble(value, nameof(ProportionOfClayInSoil));
                if (HasErrors)
                {
                    return;
                }
                BindingSoilData.ProportionOfClayInSoil = value;
            }
        }
        public double ProportionOfSandInSoil
        {
            get => BindingSoilData.ProportionOfSandInSoil;
            set
            {
                ValidateDouble(value, nameof(ProportionOfSandInSoil));
                if (HasErrors)
                {
                    return;
                }
                BindingSoilData.ProportionOfSandInSoil = value;
            }
        }
        public double SoilPh
        {
            get => BindingSoilData.SoilPh;
            set
            {
                ValidateDouble(value, nameof(SoilPh));
                if (HasErrors)
                {
                    return;
                }
                BindingSoilData.SoilPh = value;
            }
        }
        public double ProportionOfSoilOrganicCarbon
        {
            get => BindingSoilData.ProportionOfSoilOrganicCarbon;
            set
            {
                ValidateDouble(value, nameof(ProportionOfSoilOrganicCarbon));
                if (HasErrors)
                {
                    return;
                }
                BindingSoilData.ProportionOfSoilOrganicCarbon = value;
            }
        }
        public double SoilCec
        {
            get => BindingSoilData.SoilCec;
            set
            {
                ValidateDouble(value, nameof(SoilCec));
                if (HasErrors)
                {
                    return;
                }
                BindingSoilData.SoilCec = value;
                
            }
        }
        public int CarbonModellingEquilibriumYear
        {
            get => _carbonModellingEquilibriumYear;
            set
            {
                SetProperty(ref _carbonModellingEquilibriumYear, value);
                ValidateCarbonModellingEquilibriumYear();
                    if (HasErrors)
                    {
                        return;
                    }
                    ActiveFarm.CarbonModellingEquilibriumYear = value;
            }
        }
        #endregion
        #region Methods
        public void ValidateDouble(double value, string propertyName)
        {
            if(value < 0)
            {
                AddError(propertyName, "Value cannot be below 0");
            }
            else
            {
                RemoveError(propertyName);
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
        public void ManageData()
        {
            ActiveFarm = base.StorageService.GetActiveFarm();
            CarbonModellingEquilibriumYear = ActiveFarm.CarbonModellingEquilibriumYear;
            BindingSoilData = ActiveFarm.DefaultSoilData;
            BindingSoilData.PropertyChanged -= OnSoilDataPropertyChanged;
            BindingSoilData.PropertyChanged += OnSoilDataPropertyChanged;
        }
        #endregion
        #region Event Handlers
        public void OnSoilDataPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BulkDensity))
            {
                BulkDensity = ActiveFarm.DefaultSoilData.BulkDensity;
            }
            else if (e.PropertyName == nameof(TopLayerThickness))
            {
                TopLayerThickness = ActiveFarm.DefaultSoilData.TopLayerThickness;
            }
            else if (e.PropertyName == nameof(ProportionOfClayInSoil))
            {
                ProportionOfClayInSoil = ActiveFarm.DefaultSoilData.ProportionOfClayInSoil;
            }
            else if (e.PropertyName == nameof(ProportionOfSandInSoil))
            {
                ProportionOfSandInSoil = ActiveFarm.DefaultSoilData.ProportionOfSandInSoil;
            }
            else if (e.PropertyName == nameof(ProportionOfSoilOrganicCarbon))
            {
                ProportionOfSoilOrganicCarbon = ActiveFarm.DefaultSoilData.ProportionOfSoilOrganicCarbon;
            }
            else if (e.PropertyName == nameof(SoilPh))
            {
                SoilPh = ActiveFarm.DefaultSoilData.SoilPh;
            }
            else if (e.PropertyName == nameof(SoilCec))
            {
                SoilCec = ActiveFarm.DefaultSoilData.SoilCec;
            }
        }
        #endregion
    }
}
