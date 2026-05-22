using System.Collections.Generic;
using H.Core.Models;
using H.Core.Models.LandManagement.Fields;

namespace H.Core.Services.Initialization;

public interface ICropInitializationService
{
    void InitializeFuelEnergy(Farm farm);
    void InitializeFuelEnergy(Farm farm, CropViewItem viewItem);
    void InitializeHerbicideEnergy(Farm farm, CropViewItem viewItem);
    void Initialize(Farm farm);
    void Initialize(CropViewItem viewItem, Farm farm);

    /// <summary>Applies all default science inputs to a crop (the orchestrator).</summary>
    void InitializeCrop(CropViewItem viewItem, Farm farm, GlobalSettings globalSettings);

    void InitializeYield(CropViewItem viewItem, Farm farm);
    void InitializeDefaultYield(CropViewItem viewItem, Farm farm);
    void InitializeYieldForYear(Farm farm, CropViewItem viewItem, FieldSystemComponent fieldSystemComponent);
    void InitializeYieldForAllYears(IEnumerable<CropViewItem> cropViewItems, Farm farm, FieldSystemComponent fieldSystemComponent);
    void InitializeDefaultSilageCropYield(CropViewItem silageCropViewItem, Farm farm);
    double CalculateSilageCropYield(CropViewItem grainCropViewItem);

    void InitializeNitrogenFixation(CropViewItem viewItem);
    void InitializeCarbonConcentration(CropViewItem viewItem, Defaults defaults);
    void InitializeIrrigationWaterApplication(Farm farm, CropViewItem viewItem);
    void InitializeBiomassCoefficients(CropViewItem viewItem, Farm farm);
    void InitializeNitrogenContent(CropViewItem viewItem, Farm farm);
    void InitializeSoilProperties(CropViewItem viewItem, Farm farm);
    void InitializePercentageReturns(CropViewItem viewItem, Defaults defaults);
    void InitializeMoistureContent(CropViewItem cropViewItem, Farm farm);
    void InitializeTillageType(CropViewItem viewItem, Farm farm);
    void InitializeFallow(CropViewItem viewItem, Farm farm);
    void InitializePerennialDefaults(CropViewItem viewItem, Farm farm);
    void InitializeHarvestMethod(CropViewItem viewItem, Farm farm);
    void InitializeLigninContent(CropViewItem cropViewItem, Farm farm);
    void InitializeUserDefaults(CropViewItem viewItem, GlobalSettings globalSettings);
    void InitializeEconomicDefaults(CropViewItem cropViewItem, Farm farm);
    void InitializeLumCMaxValues(CropViewItem cropViewItem, Farm farm);
    void InitializeBlendData(FertilizerApplicationViewItem fertilizerApplicationViewItem);
    void InitializePhosphorusFertilizerRate(CropViewItem viewItem, Farm farm);
    void InitializeGrazingViewItems(Farm farm, CropViewItem viewItem, FieldSystemComponent fieldSystemComponent);
}
