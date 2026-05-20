#region Imports

using H.Core.Calculators.Carbon;
using H.Core.Calculators.Climate;
using H.Core.Calculators.Nitrogen;
using H.Core.Calculators.Shelterbelt;
using H.Core.Calculators.Tillage;
using H.Core.Calculators.UnitsOfMeasurement;
using H.Core.Providers;
using H.Core.Providers.Carbon;
using H.Core.Providers.Climate;
using H.Core.Providers.Energy;
using H.Core.Providers.Feed;
using H.Core.Providers.Soil;
using H.Core.Services;
using H.Core.Services.Analysis;
using H.Core.Services.Animals;
using H.Core.Services.DietService;
using H.Core.Services.Initialization;
using H.Core.Services.LandManagement;
using Prism.Ioc;
using Prism.Modularity;

#endregion

namespace H.Core
{
    /// <summary>
    /// Load services from the project into the container.
    /// </summary>
    public class CoreModule : IModule
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IClimateParameterCalculator, ClimateParameterCalculator>();
            containerRegistry.RegisterSingleton<ITillageFactorCalculator, TillageFactorCalculator>();
            containerRegistry.RegisterSingleton<IICBMSoilCarbonCalculator, ICBMSoilCarbonCalculator>();

            // Manure / digestate / field-component helpers — registered so the carbon calculators
            // below can receive them via constructor injection (and so every consumer shares one
            // instance instead of each calculator newing its own).
            containerRegistry.RegisterSingleton<IManureService, ManureService>();
            containerRegistry.RegisterSingleton<IDigestateService, DigestateService>();
            containerRegistry.RegisterSingleton<IFieldComponentHelper, FieldComponentHelper>();

            // Carbon input calculators + the orchestrating CarbonService. Registered here so non-GUI
            // consumers (CLI, tests) get the same instances as the Avalonia app. These use constructor
            // injection for their manure / digestate / animal / field-component dependencies.
            containerRegistry.RegisterSingleton<IICBMCarbonInputCalculator, ICBMCarbonInputCalculator>();
            containerRegistry.RegisterSingleton<IIPCCTier2CarbonInputCalculator, IPCCTier2CarbonInputCalculator>();
            containerRegistry.RegisterSingleton<ICarbonService, CarbonService>();

            // Phase 5: GUI vertical slice — register the soil/N calculator stack as concrete
            // singletons so FieldResultsService can be resolved via constructor injection, then
            // expose IFieldResultsService and the new IFarmAnalysisService façade.
            containerRegistry.RegisterSingleton<N2OEmissionFactorCalculator>();
            containerRegistry.RegisterSingleton<ICBMSoilCarbonCalculator>();
            containerRegistry.RegisterSingleton<IPCCTier2SoilCarbonCalculator>();
            // IAnimalService is a transitive dep of IFarmAnalysisService — without this
            // registration, Prism silently falls back to GHGResultsViewModel's parameterless
            // constructor, leaving _logger / _farmAnalysisService null and NRE'ing on first navigate.
            containerRegistry.RegisterSingleton<IAnimalService, AnimalResultsService>();
            containerRegistry.RegisterSingleton<IFieldResultsService, FieldResultsService>();
            containerRegistry.RegisterSingleton<ShelterbeltCalculator>();
            containerRegistry.RegisterSingleton<IFarmAnalysisService, FarmAnalysisService>();

            containerRegistry.RegisterSingleton<ICustomFileClimateDataProvider, CustomFileClimateDataProvider>();
            containerRegistry.RegisterSingleton<IDietProvider, DietProvider>();
            containerRegistry.RegisterSingleton<IResidueDataProvider, Table_7_Relative_Biomass_Information_Provider>();
            containerRegistry.RegisterSingleton<ISoilDataProvider, NationalSoilDataBaseProvider>();
            containerRegistry.RegisterSingleton<IGeographicDataProvider, GeographicDataProvider>();
            containerRegistry.RegisterSingleton<IFeedIngredientProvider, FeedIngredientProvider>();

            // Climate providers — needed by N2OEmissionFactorCalculator / ICBMSoilCarbonCalculator.
            containerRegistry.RegisterSingleton<IClimateProvider, ClimateProvider>();
            containerRegistry.RegisterSingleton<ISlcClimateProvider, SlcClimateDataProvider>();

            // Crop initialization (CLI ProcessCommandLineItems needs it; it depends on the host
            // providing ILogger + ICacheService, which both front-ends register separately).
            containerRegistry.RegisterSingleton<ITable50FuelEnergyEstimatesProvider, Table50FuelEnergyEstimatesProvider>();
            containerRegistry.RegisterSingleton<ICropInitializationService, CropInitializationService>();

            containerRegistry.RegisterSingleton<IUnitsOfMeasurementCalculator, UnitsOfMeasurementCalculator>();

            // Diet services
            containerRegistry.RegisterSingleton<IDietFactory, DietFactory>();
            containerRegistry.RegisterSingleton<IDietService, DefaultDietService>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        #endregion

        #region Private Methods

        #endregion

        #region Event Handlers

        #endregion
    }
}