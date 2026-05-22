using H.Core;
using H.Core.Calculators.Carbon;
using H.Core.Calculators.Economics;
using H.Core.Enumerations;
using H.Core.Models;
using H.Core.Models.LandManagement.Fields;
using H.Core.Providers.Carbon;
using H.Core.Providers.Economics;
using H.Core.Providers.Energy;
using H.Core.Providers.Fertilizer;
using H.Core.Providers.Nitrogen;
using H.Core.Providers.Plants;
using H.Core.Providers.Soil;
using H.Core.Services.LandManagement;
using H.Infrastructure.Services;
using Microsoft.Extensions.Logging;

namespace H.Core.Services.Initialization;

/// <summary>
/// Seeds a <see cref="CropViewItem"/> with the default science inputs the carbon / nitrogen
/// pipeline consumes — yield, biomass coefficients, residue nitrogen contents, moisture, lignin,
/// tillage, percentage-returns, perennial / fallow defaults, fuel + herbicide energy, economics —
/// based on the crop type, the farm's province / soil, and the user's defaults.
///
/// <para>
/// This logic previously lived on <c>FieldResultsService</c> (as <c>Assign*</c> methods); it is
/// gathered here so the place that <i>creates</i> a crop (the field editor via
/// <see cref="H.Core.Factories.Crops.ICropFactory"/>) and the place that <i>builds the per-year
/// detail tree</i> (<c>FieldResultsService.InitializeStageState</c>) both seed crops through one
/// service. The orchestrator is <see cref="InitializeCrop"/>.
/// </para>
///
/// <para><b>Provider ownership:</b></para>
/// The stateless lookup providers (yield, residue/biomass, energy, economics, etc.) are
/// constructed here. <see cref="IICBMCarbonInputCalculator"/> is injected (needed by the silage
/// yield estimate). <see cref="SmallAreaYieldProvider"/> loads its ~1M-row CSV in the constructor.
/// </summary>
public partial class CropInitializationService : ICropInitializationService
{
    #region Fields

    private readonly ILogger _logger;
    private readonly ICacheService _cahCacheService;
    private readonly ITable50FuelEnergyEstimatesProvider _table50FuelEnergyEstimatesProvider;

    // ICBM input math — used to derive the silage-crop yield from its grain-crop equivalent.
    private readonly IICBMCarbonInputCalculator _icbmCarbonInputCalculator;

    // Stateless lookup tables / helpers consulted while seeding crop defaults.
    private readonly IrrigationService _irrigationService = new IrrigationService();
    private readonly NitogenFixationProvider _nitrogenFixationProvider = new NitogenFixationProvider();
    private readonly Table_60_Utilization_Rates_For_Livestock_Grazing_Provider _utilizationRatesForLivestockGrazingProvider = new Table_60_Utilization_Rates_For_Livestock_Grazing_Provider();
    private readonly Table_51_Herbicide_Energy_Estimates_Provider _herbicideEnergyEstimatesProvider = new Table_51_Herbicide_Energy_Estimates_Provider();
    private readonly Table_48_Carbon_Footprint_For_Fertilizer_Blends_Provider _carbonFootprintForFertilizerBlendsProvider = new Table_48_Carbon_Footprint_For_Fertilizer_Blends_Provider();
    private readonly Table_9_Nitrogen_Lignin_Content_In_Crops_Provider _slopeProviderTable = new Table_9_Nitrogen_Lignin_Content_In_Crops_Provider();
    private readonly Table_7_Relative_Biomass_Information_Provider _relativeBiomassInformationProvider = new Table_7_Relative_Biomass_Information_Provider();
    private readonly EcodistrictDefaultsProvider _ecodistrictDefaultsProvider = new EcodistrictDefaultsProvider();
    private readonly LumCMax_KValues_Perennial_Cropping_Change_Provider _lumCMaxKValuesPerennialCroppingChangeProvider = new LumCMax_KValues_Perennial_Cropping_Change_Provider();
    private readonly LumCMax_KValues_Fallow_Practice_Change_Provider _lumCMaxKValuesFallowPracticeChangeProvider = new LumCMax_KValues_Fallow_Practice_Change_Provider();
    private readonly LandManagementChangeHelper _landManagementChangeHelper = new LandManagementChangeHelper();
    private readonly EconomicsHelper _economicsHelper = new EconomicsHelper();
    private readonly CropEconomicsProvider _economicsProvider = new CropEconomicsProvider();
    private readonly SmallAreaYieldProvider _smallAreaYieldProvider = new SmallAreaYieldProvider();

    #endregion

    #region Constructors

    public CropInitializationService(
        ILogger logger,
        ICacheService cahCacheService,
        ITable50FuelEnergyEstimatesProvider table50FuelEnergyEstimatesProvider,
        IICBMCarbonInputCalculator icbmCarbonInputCalculator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _cahCacheService = cahCacheService ?? throw new ArgumentNullException(nameof(cahCacheService));
        _table50FuelEnergyEstimatesProvider = table50FuelEnergyEstimatesProvider ?? throw new ArgumentNullException(nameof(table50FuelEnergyEstimatesProvider));
        _icbmCarbonInputCalculator = icbmCarbonInputCalculator ?? throw new ArgumentNullException(nameof(icbmCarbonInputCalculator));

        // Loads the ~1M-row small-area-yield CSV once for this (singleton) service.
        _smallAreaYieldProvider.Initialize();
    }

    #endregion

    #region Public Methods

    public void Initialize(Farm farm)
    {
        this.InitializeFuelEnergy(farm);
    }

    public void Initialize(CropViewItem viewItem, Farm farm)
    {
        // Full default seeding for a single crop (used at crop-creation time via the factory).
        this.InitializeCrop(viewItem, farm, new GlobalSettings());
    }

    /// <summary>
    /// Applies the default properties on a crop view item based on Holos defaults and user defaults
    /// (if available). Any property that cannot be set in the constructor of the
    /// <see cref="CropViewItem"/> should be set here.
    /// </summary>
    public void InitializeCrop(CropViewItem viewItem, Farm farm, GlobalSettings globalSettings)
    {
        viewItem.IsInitialized = false;

        _logger.LogInformation("{Service}.{Method}: applying defaults to {CropType}",
            nameof(CropInitializationService), nameof(InitializeCrop), viewItem.CropTypeString);

        var defaults = farm.Defaults;

        this.InitializeNitrogenFixation(viewItem);
        this.InitializeCarbonConcentration(viewItem, defaults);
        this.InitializeIrrigationWaterApplication(farm, viewItem);
        this.InitializeBiomassCoefficients(viewItem, farm);
        this.InitializeNitrogenContent(viewItem, farm);
        this.InitializeSoilProperties(viewItem, farm);
        this.InitializePercentageReturns(viewItem, farm.Defaults);
        this.InitializeMoistureContent(viewItem, farm);
        this.InitializeTillageType(viewItem, farm);
        this.InitializeYield(viewItem, farm);
        this.InitializeFuelEnergy(farm, viewItem);
        this.InitializeHerbicideEnergy(farm, viewItem);
        this.InitializeFallow(viewItem, farm);
        this.InitializePerennialDefaults(viewItem, farm);
        this.InitializeHarvestMethod(viewItem, farm);
        this.InitializeLigninContent(viewItem, farm);

        if (viewItem.CropType == CropType.RangelandNative)
        {
            viewItem.IsNativeGrassland = true;
        }
        else
        {
            viewItem.IsNativeGrassland = false;
        }

        this.InitializeEconomicDefaults(viewItem, farm);

        // Lastly, apply user defaults if user has specified custom values for this crop type
        this.InitializeUserDefaults(viewItem, globalSettings);

        viewItem.IsInitialized = true;
        viewItem.CropEconomicData.IsInitialized = true;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Looks up the Table 7 relative-biomass / residue row for a crop, keyed on its irrigation,
    /// crop type, and the farm's province / soil category. Returns null when there is no row.
    /// </summary>
    private Table_7_Relative_Biomass_Information_Data? GetResidueData(CropViewItem cropViewItem, Farm farm)
    {
        var soilData = farm.GetPreferredSoilData(cropViewItem);

        var residueData = _relativeBiomassInformationProvider.GetResidueData(
            irrigationType: cropViewItem.IrrigationType,
            irrigationAmount: cropViewItem.AmountOfIrrigation,
            cropType: cropViewItem.CropType,
            soilFunctionalCategory: soilData.SoilFunctionalCategory,
            province: soilData.Province);

        return residueData;
    }

    #endregion
}
