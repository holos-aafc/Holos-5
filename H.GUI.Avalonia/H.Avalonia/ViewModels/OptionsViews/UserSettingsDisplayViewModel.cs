using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExCSS;
using H.Core.Enumerations;
using H.Core.Services.StorageService;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class UserSettingsDisplayViewModel : ViewModelBase
    {
        #region Fields
        private bool _showCustomEquilibriumCarbonInput;
        private bool _showCustomRunInPeriodTillage;
        private bool _showCustomN2OEmissionFactor; 
        #endregion

        #region Constructors
        public UserSettingsDisplayViewModel(IStorageService storageService) : base(storageService)
        {
            ManageData();
            System.Diagnostics.Debug.WriteLine(ActiveFarm.Defaults.DefaultPumpType);
        }
        #endregion

        #region Properties
        //Wrapper properties for validation before setting the values
        public double CustomN2OEmissionFactor
        {
            get => ActiveFarm.Defaults.CustomN2OEmissionFactor;
            set
            {
                ValidateNonNegative(nameof(CustomN2OEmissionFactor), value);
                if (HasErrors)
                {
                    return;
                }
                ActiveFarm.Defaults.CustomN2OEmissionFactor = value;
                RaisePropertyChanged(nameof(CustomN2OEmissionFactor));
            }
        }
        public double EmissionFactorForLeachingAndRunoff
        {
            get => ActiveFarm.Defaults.EmissionFactorForLeachingAndRunoff;
            set
            {
                ValidateNonNegative(nameof(EmissionFactorForLeachingAndRunoff), value);
                if (HasErrors)
                {
                    return;
                }
                ActiveFarm.Defaults.EmissionFactorForLeachingAndRunoff = value;
                RaisePropertyChanged(nameof(EmissionFactorForLeachingAndRunoff));
            }
        }
        public double EmissionFactorForVolatilization
        {
            get => ActiveFarm.Defaults.EmissionFactorForVolatilization;
            set
            {
                ValidateNonNegative(nameof(EmissionFactorForVolatilization), value);
                if (HasErrors)
                {
                    return;
                }
                ActiveFarm.Defaults.EmissionFactorForVolatilization = value;
                RaisePropertyChanged(nameof(EmissionFactorForVolatilization));
            }
        }
        public double DefaultSupplementalFeedingLossPercentage
        {
            get => ActiveFarm.Defaults.DefaultSupplementalFeedingLossPercentage;
            set
            {
                ValidateNonNegative(nameof(DefaultSupplementalFeedingLossPercentage), value);
                if (HasErrors)
                {
                    return;
                }
                ActiveFarm.Defaults.DefaultSupplementalFeedingLossPercentage = value;
                RaisePropertyChanged(nameof(DefaultSupplementalFeedingLossPercentage));
            }
        }
        public double PercentageOfStrawReturnedToSoilForRootCrops
        {
            get => ActiveFarm.Defaults.PercentageOfStrawReturnedToSoilForRootCrops;
            set
            {
                ValidatePercentage(nameof(PercentageOfStrawReturnedToSoilForRootCrops), value);
                if (HasErrors)
                {
                    return;
                }
                ActiveFarm.Defaults.PercentageOfStrawReturnedToSoilForRootCrops = value;
                RaisePropertyChanged(nameof(PercentageOfStrawReturnedToSoilForRootCrops));
            }
        }
        public double PercentageOfProductReturnedToSoilForRootCrops
        {
            get => ActiveFarm.Defaults.PercentageOfProductReturnedToSoilForRootCrops;
            set
            {
                ValidatePercentage(nameof(PercentageOfProductReturnedToSoilForRootCrops), value);
                if (HasErrors)
                {
                    return;
                }
                ActiveFarm.Defaults.PercentageOfProductReturnedToSoilForRootCrops = value;
                RaisePropertyChanged(nameof(PercentageOfProductReturnedToSoilForRootCrops));
            }
        }
        public double PercentageOfRootsReturnedToSoilForPerennials
        {
            get => ActiveFarm.Defaults.PercentageOfRootsReturnedToSoilForPerennials;
            set
            {
                ValidatePercentage(nameof(PercentageOfRootsReturnedToSoilForPerennials), value);
                if (HasErrors)
                {
                    return;
                }
                ActiveFarm.Defaults.PercentageOfRootsReturnedToSoilForPerennials = value;
                RaisePropertyChanged(nameof(PercentageOfRootsReturnedToSoilForPerennials));
            }
        }
        public double PercentageOfProductReturnedToSoilForPerennials
        {
            get => ActiveFarm.Defaults.PercentageOfProductReturnedToSoilForPerennials;
            set
            {
                ValidatePercentage(nameof(PercentageOfProductReturnedToSoilForPerennials), value);
                if (HasErrors)
                {
                    return;
                }
                ActiveFarm.Defaults.PercentageOfProductReturnedToSoilForPerennials = value;
                RaisePropertyChanged(nameof(PercentageOfProductReturnedToSoilForPerennials));
            }
        }
        public double PercentageOfRootsReturnedToSoilForFodderCorn
        {
            get => ActiveFarm.Defaults.PercentageOfRootsReturnedToSoilForFodderCorn;
            set
            {
                ValidatePercentage(nameof(PercentageOfRootsReturnedToSoilForFodderCorn), value);
                if (HasErrors)
                {
                    return;
                }
                ActiveFarm.Defaults.PercentageOfRootsReturnedToSoilForFodderCorn = value;
                RaisePropertyChanged(nameof(PercentageOfRootsReturnedToSoilForFodderCorn));
            }
        }
        public double PercentageOfProductReturnedToSoilForFodderCorn
        {
            get => ActiveFarm.Defaults.PercentageOfProductReturnedToSoilForFodderCorn;
            set
            {
                ValidatePercentage(nameof(PercentageOfProductReturnedToSoilForFodderCorn), value);
                if (HasErrors)
                {
                    return;
                }
                ActiveFarm.Defaults.PercentageOfProductReturnedToSoilForFodderCorn = value;
                RaisePropertyChanged(nameof(PercentageOfProductReturnedToSoilForFodderCorn));
            }
        }
        public double PercentageOfRootsReturnedToSoilForAnnuals
        {
            get => ActiveFarm.Defaults.PercentageOfRootsReturnedToSoilForAnnuals;
            set
            {
                ValidatePercentage(nameof(PercentageOfRootsReturnedToSoilForAnnuals), value);
                if (HasErrors)
                {
                    return;
                }
                ActiveFarm.Defaults.PercentageOfRootsReturnedToSoilForAnnuals = value;
                RaisePropertyChanged(nameof(PercentageOfRootsReturnedToSoilForAnnuals));
            }
        }
        public int DefaultRunInPeriod
        {
            get => ActiveFarm.Defaults.DefaultRunInPeriod;
            set
            {
                ValidateNonNegative(nameof(DefaultRunInPeriod), value);
                if (HasErrors)
                {
                    return;
                }
                ActiveFarm.Defaults.DefaultRunInPeriod = value;
                RaisePropertyChanged(nameof(DefaultRunInPeriod));
            }
        }
        public double PercentageOfProductReturnedToSoilForAnnuals
        {
            get => ActiveFarm.Defaults.PercentageOfProductReturnedToSoilForAnnuals;
            set
            {
                ValidatePercentage(nameof(PercentageOfProductReturnedToSoilForAnnuals), value);
                if (HasErrors)
                {
                    return;
                }
                ActiveFarm.Defaults.PercentageOfProductReturnedToSoilForAnnuals = value;
                RaisePropertyChanged(nameof(PercentageOfProductReturnedToSoilForAnnuals));
            }
        }
        public double PercentageOfStrawReturnedToSoilForAnnuals
        {
            get => ActiveFarm.Defaults.PercentageOfStrawReturnedToSoilForAnnuals;
            set
            {
                ValidatePercentage(nameof(PercentageOfStrawReturnedToSoilForAnnuals), value);
                if (HasErrors)
                {
                    return;
                }
                ActiveFarm.Defaults.PercentageOfStrawReturnedToSoilForAnnuals = value;
                RaisePropertyChanged(nameof(PercentageOfStrawReturnedToSoilForAnnuals));
            }
        }
        public double CustomEquilibriumCarbonValue
        {
            get => ActiveFarm.StartingSoilOrganicCarbon;
            set
            {
                ValidateNonNegative(nameof(CustomEquilibriumCarbonValue), value);
                if (HasErrors)
                {
                    return;
                }
                ActiveFarm.StartingSoilOrganicCarbon = value;
                RaisePropertyChanged(nameof(CustomEquilibriumCarbonValue));
            } 
        }

        //Display the custom equilibrium carbon input field if the user selects the custom option
        public bool ShowCustomEquilibriumCarbonInput
        {
            get => _showCustomEquilibriumCarbonInput;
            set => SetProperty(ref _showCustomEquilibriumCarbonInput, value);
        }

        //Display the custom N2O emission factor input field if the user selects the custom option
        public bool ShowCustomN2OEmissionFactor
        {
            get => _showCustomN2OEmissionFactor;
            set => SetProperty(ref _showCustomN2OEmissionFactor, value);
        }

        //Display the custom run-in period tillage input field if the user selects the custom option
        public bool ShowCustomRunInPeriodTillage
        {
            get => _showCustomRunInPeriodTillage;
            set => SetProperty(ref _showCustomRunInPeriodTillage, value);
        }

        //Collections for ComboBox
        public ObservableCollection<CarbonModellingStrategies> CarbonModellingStrategiesList { get; set; }
        public ObservableCollection<EquilibriumCalculationStrategies> EquilibriumCalculationStrategiesList { get; set; }
        public ObservableCollection<TillageType> RunInPeriodTillageList { get; set; }
        public ObservableCollection<PumpType> PumpTypeList { get; set; }
        #endregion
        #region Methods
        public void ManageData()
        {
            // Populate the CarbonModellingStrategiesList with all CarbonModellingStrategies values
            CarbonModellingStrategiesList = new ObservableCollection<CarbonModellingStrategies>();
            var carbonModellingStrategies = CarbonModellingStrategies.GetValues(typeof(CarbonModellingStrategies));
            foreach (CarbonModellingStrategies carbonModellingStrategy in carbonModellingStrategies)
            {
                if (!CarbonModellingStrategiesList.Contains(carbonModellingStrategy))
                {
                    CarbonModellingStrategiesList.Add(carbonModellingStrategy);
                }
            }

            // Populate the EquilibriumCalculationStrategiesList with all EquilibriumCalculationStrategies values
            EquilibriumCalculationStrategiesList = new ObservableCollection<EquilibriumCalculationStrategies>();
            var equilibriumCalculationStrategies = EquilibriumCalculationStrategies.GetValues(typeof(EquilibriumCalculationStrategies));
            foreach (EquilibriumCalculationStrategies equilibriumCalculationStrategy in equilibriumCalculationStrategies)
            {
                if (!EquilibriumCalculationStrategiesList.Contains(equilibriumCalculationStrategy))
                {
                    EquilibriumCalculationStrategiesList.Add(equilibriumCalculationStrategy);
                }
            }

            // Populate the RunInPeriodTillageList with all TillageType values
            RunInPeriodTillageList = new ObservableCollection<TillageType>();
            var tillageTypes = TillageType.GetValues(typeof(TillageType));
            foreach (TillageType tillageType in tillageTypes)
            {
                if (!RunInPeriodTillageList.Contains(tillageType))
                {
                    RunInPeriodTillageList.Add(tillageType);
                }
            }

            // Populate the PumpTypeList with all PumpType values
            PumpTypeList = new ObservableCollection<PumpType>();
            var pumpTypes = PumpType.GetValues(typeof(PumpType));
            foreach (PumpType pumpType in pumpTypes)
            {
                if (!PumpTypeList.Contains(pumpType))
                {
                    PumpTypeList.Add(pumpType);
                }
            }
        }

        //Validates the value if it is negative
        public void ValidateNonNegative(string propertyName, double value)
        {
            if (value < 0)
            {
                AddError(propertyName, H.Core.Properties.Resources.ErrorMustBeGreaterThan0);
            }
            else
            {
                RemoveError(propertyName);
            }
        }

        //Validates the value if it is not between 0 and 100
        public void ValidatePercentage(string propertyName, double value)
        {
            if (value < 0 || value > 100)
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
