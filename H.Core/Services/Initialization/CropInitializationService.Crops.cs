using H.Core.Enumerations;
using H.Core.Mappers;
using H.Core.Models;
using H.Core.Models.Animals;
using H.Core.Models.LandManagement.Fields;
using H.Core.Providers.Carbon;

namespace H.Core.Services.Initialization;

public partial class CropInitializationService
{
    #region Public Methods

    public void InitializeNitrogenFixation(CropViewItem viewItem)
    {
        viewItem.NitrogenFixationPercentage = _nitrogenFixationProvider.GetNitrogenFixationResult(viewItem.CropType).Fixation * 100;
    }

    public void InitializeCarbonConcentration(CropViewItem viewItem, Defaults defaults)
    {
        viewItem.CarbonConcentration = defaults.CarbonConcentration;
    }

    public void InitializeIrrigationWaterApplication(Farm farm, CropViewItem viewItem)
    {
        viewItem.AmountOfIrrigation = _irrigationService.GetDefaultIrrigationForYear(farm, viewItem.Year);
        viewItem.GrowingSeasonIrrigation = _irrigationService.GetGrowingSeasonIrrigation(farm, viewItem);
    }

    public void InitializeBiomassCoefficients(CropViewItem viewItem, Farm farm)
    {
        var residueData = this.GetResidueData(viewItem, farm);
        if (residueData != null)
        {
            viewItem.BiomassCoefficientProduct = residueData.RelativeBiomassProduct;
            viewItem.BiomassCoefficientStraw = residueData.RelativeBiomassStraw;
            viewItem.BiomassCoefficientRoots = residueData.RelativeBiomassRoot;
            viewItem.BiomassCoefficientExtraroot = residueData.RelativeBiomassExtraroot;

            if (viewItem.HarvestMethod == HarvestMethods.Swathing || viewItem.HarvestMethod == HarvestMethods.GreenManure || viewItem.HarvestMethod == HarvestMethods.Silage)
            {
                viewItem.BiomassCoefficientProduct = residueData.RelativeBiomassProduct + residueData.RelativeBiomassStraw;
                viewItem.BiomassCoefficientStraw = 0;
                viewItem.BiomassCoefficientRoots = residueData.RelativeBiomassRoot;
                viewItem.BiomassCoefficientExtraroot = residueData.RelativeBiomassExtraroot;
            }
        }
    }

    public void InitializeNitrogenContent(CropViewItem viewItem, Farm farm)
    {
        // Assign N content values used for the ICBM methodology
        var residueData = this.GetResidueData(viewItem, farm);
        if (residueData != null)
        {
            // Table has values in grams but unit of display is kg
            viewItem.NitrogenContentInProduct = residueData.NitrogenContentProduct / 1000;
            viewItem.NitrogenContentInStraw = residueData.NitrogenContentStraw / 1000;
            viewItem.NitrogenContentInRoots = residueData.NitrogenContentRoot / 1000;
            viewItem.NitrogenContentInExtraroot = residueData.NitrogenContentExtraroot / 1000;

            if (viewItem.CropType.IsPerennial())
            {
                viewItem.NitrogenContentInStraw = 0;
            }
        }

        // Assign N content values used for IPCC Tier 2
        var cropData = _slopeProviderTable.GetDataByCropType(viewItem.CropType);
        viewItem.NitrogenContent = cropData.NitrogenContentResidues;
    }

    public void InitializeSoilProperties(CropViewItem viewItem, Farm farm)
    {
        var soilData = farm.GetPreferredSoilData(viewItem);

        viewItem.Sand = soilData.ProportionOfSandInSoil;
    }

    /// <summary>
    /// Assigns default percentage-return-to-soil values for a <see cref="CropViewItem"/>.
    /// </summary>
    public void InitializePercentageReturns(CropViewItem viewItem, Defaults defaults)
    {
        if (viewItem.CropType.IsPerennial())
        {
            viewItem.PercentageOfProductYieldReturnedToSoil = defaults.PercentageOfProductReturnedToSoilForPerennials;
            viewItem.PercentageOfRootsReturnedToSoil = defaults.PercentageOfRootsReturnedToSoilForPerennials;
        }
        else if (viewItem.CropType.IsSilageCrop())
        {
            viewItem.PercentageOfProductYieldReturnedToSoil = defaults.PercentageOfProductReturnedToSoilForFodderCorn;
            viewItem.PercentageOfStrawReturnedToSoil = defaults.PercentageOfRootsReturnedToSoilForFodderCorn;
        }
        else if (viewItem.CropType.IsAnnual())
        {
            viewItem.PercentageOfProductYieldReturnedToSoil = defaults.PercentageOfProductReturnedToSoilForAnnuals;
            viewItem.PercentageOfRootsReturnedToSoil = defaults.PercentageOfRootsReturnedToSoilForAnnuals;
            viewItem.PercentageOfStrawReturnedToSoil = defaults.PercentageOfStrawReturnedToSoilForAnnuals;
        }

        if (viewItem.CropType.IsRootCrop())
        {
            viewItem.PercentageOfProductYieldReturnedToSoil = defaults.PercentageOfProductReturnedToSoilForRootCrops;
            viewItem.PercentageOfStrawReturnedToSoil = defaults.PercentageOfStrawReturnedToSoilForRootCrops;
        }

        if (viewItem.CropType.IsCoverCrop())
        {
            viewItem.PercentageOfProductYieldReturnedToSoil = 100;
            viewItem.PercentageOfStrawReturnedToSoil = 100;
            viewItem.PercentageOfRootsReturnedToSoil = 100;
        }

        if (viewItem.HarvestMethod == HarvestMethods.Silage)
        {
            viewItem.PercentageOfProductYieldReturnedToSoil = 2;
            viewItem.PercentageOfStrawReturnedToSoil = 0;
            viewItem.PercentageOfRootsReturnedToSoil = 100;
        }
        else if (viewItem.HarvestMethod == HarvestMethods.Swathing)
        {
            viewItem.PercentageOfProductYieldReturnedToSoil = 30;
            viewItem.PercentageOfStrawReturnedToSoil = 0;
            viewItem.PercentageOfRootsReturnedToSoil = 100;
        }
        else if (viewItem.HarvestMethod == HarvestMethods.GreenManure)
        {
            viewItem.PercentageOfProductYieldReturnedToSoil = 100;
            viewItem.PercentageOfStrawReturnedToSoil = 0;
            viewItem.PercentageOfRootsReturnedToSoil = 100;
        }
    }

    public void InitializeMoistureContent(CropViewItem cropViewItem, Farm farm)
    {
        var residueData = this.GetResidueData(cropViewItem, farm);

        if (cropViewItem.HarvestMethod == HarvestMethods.GreenManure ||
            cropViewItem.HarvestMethod == HarvestMethods.Silage ||
            cropViewItem.HarvestMethod == HarvestMethods.Swathing ||
            cropViewItem.CropType.IsSilageCrop())
        {
            cropViewItem.MoistureContentOfCropPercentage = 65;
        }
        else
        {
            if (residueData != null)
            {
                cropViewItem.MoistureContentOfCropPercentage = residueData.MoistureContentOfProduct;
            }
            else
            {
                cropViewItem.MoistureContentOfCropPercentage = 12;
            }
        }
    }

    /// <summary>
    /// Sets the tillage type (current &amp; past) for a view item based on the province.
    /// </summary>
    public void InitializeTillageType(CropViewItem viewItem, Farm farm)
    {
        var soilData = farm.GetPreferredSoilData(viewItem);

        var province = soilData.Province;
        var residueData = this.GetResidueData(viewItem, farm);
        if (residueData != null)
        {
            if (residueData.TillageTypeTable.ContainsKey(province))
            {
                var tillageTypeForProvince = residueData.TillageTypeTable[province];

                viewItem.TillageType = tillageTypeForProvince;
                viewItem.PastTillageType = tillageTypeForProvince;
            }
        }

        if (viewItem.CropType.IsPerennial())
        {
            viewItem.TillageType = TillageType.NoTill;
            viewItem.PastTillageType = TillageType.NoTill;
        }
    }

    public void InitializeFallow(CropViewItem viewItem, Farm farm)
    {
        if (viewItem.CropType.IsFallow())
        {
            viewItem.Yield = 0;
            viewItem.TillageType = farm.Defaults.DefaultTillageTypeForFallow;
            viewItem.PastTillageType = TillageType.NoTill;
            viewItem.HarvestMethod = HarvestMethods.None;
            viewItem.PercentageOfProductYieldReturnedToSoil = 0;
            viewItem.PercentageOfStrawReturnedToSoil = 0;
            viewItem.PercentageOfRootsReturnedToSoil = 0;
        }
    }

    public void InitializePerennialDefaults(CropViewItem viewItem, Farm farm)
    {
        if (viewItem.CropType.IsPerennial())
        {
            viewItem.TillageType = TillageType.NoTill;
            viewItem.PastTillageType = TillageType.NoTill;
            viewItem.FertilizerApplicationMethodology = FertilizerApplicationMethodologies.Broadcast;
            viewItem.ForageUtilizationRate = _utilizationRatesForLivestockGrazingProvider.GetUtilizationRate(viewItem.CropType);
            viewItem.TotalBiomassHarvest = viewItem.DefaultYield;
        }
    }

    public void InitializeHarvestMethod(CropViewItem viewItem, Farm farm)
    {
        if (viewItem.CropType.IsSilageCrop())
        {
            viewItem.HarvestMethod = HarvestMethods.Silage;
        }
        else
        {
            viewItem.HarvestMethod = HarvestMethods.CashCrop;
        }
    }

    public void InitializeLigninContent(CropViewItem cropViewItem, Farm farm)
    {
        var residueData = this.GetResidueData(cropViewItem, farm);

        if (residueData != null)
        {
            cropViewItem.LigninContent = residueData.LigninContent;
        }
        else
        {
            cropViewItem.LigninContent = 0.0;
        }
    }

    public void InitializeUserDefaults(CropViewItem viewItem, GlobalSettings globalSettings)
    {
        // Check if user has defaults defined for the type of crop
        var cropDefaults = globalSettings.CropDefaults.SingleOrDefault(x => x.CropType == viewItem.CropType);
        if (cropDefaults is null)
        {
            return;
        }

        if (cropDefaults.EnableCustomUserDefaultsForThisCrop == false)
        {
            // User did not specify defaults for this crop (or just wants to use system defaults) so return from here without modifying the view item further
            return;
        }

        PropertyMapper.CopyTo(cropDefaults, viewItem);
    }

    public void InitializeEconomicDefaults(CropViewItem cropViewItem, Farm farm)
    {
        var soilData = farm.GetPreferredSoilData(cropViewItem);

        cropViewItem.CropEconomicData.IsInitialized = false;

        cropViewItem.CropEconomicData = _economicsProvider.Get(
            cropType: cropViewItem.CropType,
            soilFunctionalCategory: soilData.SoilFunctionalCategory,
            province: soilData.Province);

        _economicsHelper.ConvertValuesToMetricIfNecessary(cropViewItem.CropEconomicData, farm);

        cropViewItem.CropEconomicData.IsInitialized = true;
    }

    public void InitializeLumCMaxValues(CropViewItem cropViewItem, Farm farm)
    {
        if (!cropViewItem.CropType.IsPerennial() && !cropViewItem.CropType.IsGrassland() && !cropViewItem.CropType.IsFallow() && !cropViewItem.IsBrokenGrassland)
        {
            return;
        }

        var lumCMax = 0d;
        var kValue = 0d;

        var ecozone = _ecodistrictDefaultsProvider.GetEcozone(farm.GeographicData.DefaultSoilData.EcodistrictId);

        if (cropViewItem.CropType.IsPerennial() || cropViewItem.IsBrokenGrassland)
        {
            var changeType = _landManagementChangeHelper.GetPerennialCroppingChangeType(cropViewItem.PastPerennialArea, cropViewItem.Area);
            if (cropViewItem.IsBrokenGrassland)
            {
                // From v3, if is broken grassland then use values for decrease in area when looking up lumc and k
                changeType = PerennialCroppingChangeType.DecreaseInPerennialCroppingArea;
            }

            lumCMax = _lumCMaxKValuesPerennialCroppingChangeProvider.GetLumCMax(ecozone, farm.GeographicData.DefaultSoilData.SoilTexture, changeType);
            kValue = _lumCMaxKValuesPerennialCroppingChangeProvider.GetKValue(ecozone, farm.GeographicData.DefaultSoilData.SoilTexture, changeType);
        }
        else if (cropViewItem.CropType.IsFallow())
        {
            var changeType = _landManagementChangeHelper.GetFallowPracticeChangeType(cropViewItem.PastFallowArea, cropViewItem.Area);

            lumCMax = _lumCMaxKValuesFallowPracticeChangeProvider.GetLumCMax(ecozone, farm.GeographicData.DefaultSoilData.SoilTexture, changeType);
            kValue = _lumCMaxKValuesFallowPracticeChangeProvider.GetKValue(ecozone, farm.GeographicData.DefaultSoilData.SoilTexture, changeType);
        }

        cropViewItem.LumCMax = lumCMax;
        cropViewItem.KValue = kValue;
    }

    public void InitializeBlendData(FertilizerApplicationViewItem fertilizerApplicationViewItem)
    {
        var data = _carbonFootprintForFertilizerBlendsProvider.GetData(fertilizerApplicationViewItem.FertilizerBlendData.FertilizerBlend);
        if (data is not null)
        {
            /*
             * Don't reassign the FertilizerBlendData property to the object returned from the provider since the view model will have attached event handlers
             * that will get lost of the object is assigned, instead copy individual properties
             */

            fertilizerApplicationViewItem.FertilizerBlendData.PercentageNitrogen = data.PercentageNitrogen;
            fertilizerApplicationViewItem.FertilizerBlendData.PercentagePhosphorus = data.PercentagePhosphorus;
            fertilizerApplicationViewItem.FertilizerBlendData.PercentagePotassium = data.PercentagePotassium;
            fertilizerApplicationViewItem.FertilizerBlendData.PercentageSulphur = data.PercentageSulphur;
            fertilizerApplicationViewItem.FertilizerBlendData.ApplicationEmissions = data.ApplicationEmissions;
            fertilizerApplicationViewItem.FertilizerBlendData.CarbonDioxideEmissionsAtTheGate = data.CarbonDioxideEmissionsAtTheGate;
        }
    }

    /// <summary>
    /// When not using a blended P fertilizer approach, use this to assign a P rate directly to the crop.
    /// </summary>
    public void InitializePhosphorusFertilizerRate(CropViewItem viewItem, Farm farm)
    {
        var soilData = farm.GetPreferredSoilData(viewItem);
        var province = soilData.Province;

        // Start with a default then get lookup value if one is available
        viewItem.PhosphorusFertilizerRate = 25;

        var residueData = this.GetResidueData(viewItem, farm);
        if (residueData != null)
        {
            if (residueData.PhosphorusFertilizerRateTable.ContainsKey(province))
            {
                var phosphorusFertilizerTable = residueData.PhosphorusFertilizerRateTable[province];
                if (phosphorusFertilizerTable.ContainsKey(soilData.SoilFunctionalCategory))
                {
                    var rate = phosphorusFertilizerTable[soilData.SoilFunctionalCategory];
                    viewItem.PhosphorusFertilizerRate = rate;
                }
            }
        }
    }

    /// <summary>
    /// Rebuilds a crop's grazing view items from the farm's animal management periods that place
    /// animals on pasture on this field. The GUI builds these interactively; command-line farms
    /// have no such step, so they must be generated before the grazing carbon/nitrogen
    /// contributions (manure on pasture, supplemental feed) can be calculated.
    /// </summary>
    public void InitializeGrazingViewItems(Farm farm, CropViewItem viewItem, FieldSystemComponent fieldSystemComponent)
    {
        var existingItems = new List<GrazingViewItem>();
        var newItems = new List<GrazingViewItem>();

        foreach (var animalComponent in farm.AnimalComponents)
        {
            foreach (var animalGroup in animalComponent.Groups)
            {
                foreach (var managementPeriod in animalGroup.ManagementPeriods)
                {
                    // Only management periods that put the animals on this field's pasture.
                    if (managementPeriod.HousingDetails.PastureLocation == null ||
                        managementPeriod.HousingDetails.HousingType != HousingType.Pasture ||
                        managementPeriod.HousingDetails.PastureLocation.Guid.Equals(fieldSystemComponent.Guid) == false)
                    {
                        continue;
                    }

                    var grazingViewItem = new GrazingViewItem();
                    this.InitializeGrazingViewItem(grazingViewItem, managementPeriod, animalComponent, animalGroup, viewItem);

                    var alreadyPresent = viewItem.GrazingViewItems.SingleOrDefault(x =>
                        x.AnimalComponentGuid == animalComponent.Guid &&
                        x.ManagementPeriodGuid == managementPeriod.Guid &&
                        x.AnimalGroupGuid == animalGroup.Guid &&
                        x.Start.Date == managementPeriod.Start.Date &&
                        x.End.Date == managementPeriod.End.Date);

                    if (alreadyPresent != null)
                    {
                        existingItems.Add(alreadyPresent);
                    }
                    else
                    {
                        newItems.Add(grazingViewItem);
                    }
                }
            }
        }

        viewItem.GrazingViewItems.Clear();
        foreach (var grazingViewItem in existingItems)
        {
            viewItem.GrazingViewItems.Add(grazingViewItem);
        }
        foreach (var grazingViewItem in newItems)
        {
            viewItem.GrazingViewItems.Add(grazingViewItem);
        }
    }

    #endregion

    #region Private Methods

    private void InitializeGrazingViewItem(
        GrazingViewItem grazingViewItem,
        ManagementPeriod managementPeriod,
        AnimalComponentBase animalComponent,
        AnimalGroup animalGroup,
        CropViewItem cropViewItem)
    {
        grazingViewItem.Start = managementPeriod.Start;
        grazingViewItem.End = managementPeriod.End;
        grazingViewItem.ForageActivity = ForageActivities.Grazed;
        grazingViewItem.AnimalComponentGuid = animalComponent.Guid;
        grazingViewItem.ManagementPeriodGuid = managementPeriod.Guid;
        grazingViewItem.AnimalGroupGuid = animalGroup.Guid;
        grazingViewItem.ManagementPeriodName = managementPeriod.Name;

        // Table 9 moisture content for grazed fields.
        grazingViewItem.MoistureContentAsPercentage = 80;

        grazingViewItem.Utilization = _utilizationRatesForLivestockGrazingProvider.GetUtilizationRate(cropViewItem.CropType);
    }

    #endregion
}
