using H.Core.Emissions.Results;
using H.Core.Enumerations;
using H.Core.Models;
using H.Core.Models.Animals;
using H.Core.Models.LandManagement.Fields;
using NLog;

namespace H.Core.Services.LandManagement
{
    /// <summary>
    /// Partial: carbon-input dispatch and final ICBM / Tier 2 result pass.
    ///
    /// <para><b>Two entry points worth knowing about:</b></para>
    /// <list type="bullet">
    ///   <item>
    ///     <c>AssignCarbonInputs(viewItems, farm, fieldSystemComponent)</c> — per-crop loop. For
    ///     each crop in each year, dispatches to either
    ///     <see cref="H.Core.Calculators.Carbon.IPCCTier2SoilCarbonCalculator.CalculateInputs"/>
    ///     (if strategy is IPCC Tier 2 and Table 9 has data for this crop) or
    ///     <see cref="H.Core.Calculators.Carbon.ICBMSoilCarbonCalculator.SetCarbonInputs"/>
    ///     otherwise. Also computes climate / tillage / management factors.
    ///   </item>
    ///   <item>
    ///     <c>CalculateFinalResultsForField</c> — the final per-year pool dynamics. ICBM seeds
    ///     pools from <c>ICBMSoilCarbonCalculator.CalculateEquilibriumYear</c> then steps year-by-year via
    ///     <c>ICBMSoilCarbonCalculator.CalculateCarbonAtInterval</c>; Tier 2 hands off to
    ///     <see cref="H.Core.Calculators.Carbon.IPCCTier2SoilCarbonCalculator.CalculateResults"/>.
    ///   </item>
    /// </list>
    /// </summary>
    public partial class FieldResultsService
    {
        #region Public Methods

        /// <summary>
        /// Returns a year's previous / current / next <see cref="CropViewItem"/>s (needed by the
        /// perennial missing-yield interpolation and root-return continuity). Delegates to the
        /// field-component helper, which owns this lookup.
        /// </summary>
        public AdjoiningYears GetAdjoiningYears(
            IEnumerable<CropViewItem> viewItems,
            int year)
        {
            return _fieldComponentHelper.GetAdjoiningYears(viewItems, year);
        }

        /// <summary>
        /// Before carbon change can be calculated, all view items must have yields assigned so that we can determine the total carbon inputs from all crops, manure applications, supplemental 
        /// hay applications, etc. Then we can proceed to the actual carbon change calculations.
        /// </summary>
        public void AssignCarbonInputs(
            IEnumerable<CropViewItem> viewItems,
            Farm farm,
            FieldSystemComponent fieldSystemComponent)
        {
            // Yields must be assigned to all items before we can loop over each year and calculate plant carbon in agricultural product (C_p)
            _cropInitializationService.InitializeYieldForAllYears(
                cropViewItems: viewItems,
                farm: farm, fieldSystemComponent: fieldSystemComponent);

            // After yields have been set, we must consider perennial years in which there is 0 for the yield input (from user or by default yield provider)
            this.UpdatePercentageReturnsForPerennials(
                viewItems: viewItems);

            var mainCrops = viewItems.OrderBy(x => x.Year).Where(x => x.IsSecondaryCrop == false).ToList();
            var distinctYears = mainCrops.Select(x => x.Year).Distinct().OrderBy(x => x).ToList();

            // Consider the main crops for each year in the sequence
            foreach (var year in distinctYears)
            {
                var adjoiningYears = this.GetAdjoiningYears(mainCrops, year);

                var previousYearViewItem = adjoiningYears.PreviousYearViewItem;
                // Non-null here: the loop iterates the distinct years of mainCrops, so every year
                // resolves to a main crop. (Canonical AdjoiningYears.CurrentYearViewItem is nullable.)
                var currentYearViewItem = adjoiningYears.CurrentYearViewItem!;
                var nextYearViewItem = adjoiningYears.NextYearViewItem;

                if (farm.Defaults.CarbonModellingStrategy == CarbonModellingStrategies.IPCCTier2 &&
                    _tier2SoilCarbonCalculator.CanCalculateInputsForCrop(currentYearViewItem))
                {
                    _tier2SoilCarbonCalculator.CalculateInputs(
                        viewItem: currentYearViewItem, farm: farm);
                }
                else
                {
                    _icbmCarbonInputCalculator.AssignInputs(
                        previousYearViewItem: previousYearViewItem!,
                        currentYearViewItem: currentYearViewItem,
                        nextYearViewItem: nextYearViewItem!,
                        farm: farm,
                        animalResults: this.AnimalResults);
                }

                currentYearViewItem.ClimateParameter = this.CalculateClimateParameter(currentYearViewItem, farm);

                currentYearViewItem.TillageFactor = this.CalculateTillageFactor(currentYearViewItem, farm);

                currentYearViewItem.ManagementFactor =
                    this.CalculateManagementFactor(currentYearViewItem.ClimateParameter,
                        currentYearViewItem.TillageFactor);
            }

            // Consider the secondary crops
            var secondaryCrops = viewItems.OrderBy(x => x.Year).Where(x => x.IsSecondaryCrop).ToList();
            foreach (var secondaryCrop in secondaryCrops)
            {
                if (farm.Defaults.CarbonModellingStrategy == CarbonModellingStrategies.IPCCTier2 &&
                    _tier2SoilCarbonCalculator.CanCalculateInputsForCrop(secondaryCrop))
                {
                    _tier2SoilCarbonCalculator.CalculateInputs(
                        viewItem: secondaryCrop, farm: farm);
                }
                else
                {
                    _icbmCarbonInputCalculator.AssignInputs(
                        previousYearViewItem: null!,
                        currentYearViewItem: secondaryCrop,
                        nextYearViewItem: null!,
                        farm: farm,
                        animalResults: this.AnimalResults);
                }
            }
        }

        /// <summary>
        /// If there is a year where a perennial crop has a 0 yield, it means it wasn't harvested that year. Therefore, when a perennial year has a 0 yield,
        /// we must set the percentage of product returned to soil to 100% (instead of the default 35% for perennials) since everything stayed in the field that year.
        ///
        /// This method has to be called after we assign yields.
        /// </summary>
        public void UpdatePercentageReturnsForPerennials(IEnumerable<CropViewItem> viewItems)
        {
            foreach (var cropViewItem in viewItems)
            {
                if (cropViewItem.CropType.IsPerennial())
                {
                    if (cropViewItem.Yield == 0)
                    {
                        cropViewItem.PercentageOfProductYieldReturnedToSoil =
                            100; // Now C inputs will be calculated correctly for this year
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Calculates the average soil organic carbon value for all fields on the farm.
        /// </summary>
        private void CalculateAverageSoilOrganicCarbonForFields(
            IEnumerable<CropViewItem> viewItems)
        {
            var distinctYears = viewItems.Select(x => x.Year).Distinct();
            foreach (var year in distinctYears)
            {
                // Ge all view items that have the same year.
                var viewItemsByYear = viewItems.Where(x => x.Year == year);

                // Get the average soil organic carbon from items.
                var averageSoilOrganicCarbon = viewItemsByYear.Average(x => x.SoilCarbon);

                // Assign this common value to each item.
                foreach (var viewItem in viewItemsByYear)
                {
                    viewItem.AverageSoilCarbonAcrossAllFieldsInFarm = averageSoilOrganicCarbon;
                }
            }
        }

        private FieldSystemComponent? GetLeftMostComponent(FieldSystemComponent fieldSystemComponent, Farm farm)
        {
            var currentFieldComponent = new FieldSystemComponent();

            var currentComponentId = fieldSystemComponent.CurrentPeriodComponentGuid;
            if (currentComponentId.Equals(Guid.Empty))
            {
                currentFieldComponent = fieldSystemComponent;
            }
            else
            {
                currentFieldComponent = farm.GetFieldSystemComponent(currentComponentId);
            }

            if (currentFieldComponent != null && currentFieldComponent.HistoricalComponents.Any())
            {
                return currentFieldComponent.HistoricalComponents.Cast<FieldSystemComponent>().OrderBy(x => x.StartYear)
                    .First();
            }
            else
            {
                return currentFieldComponent;
            }
        }

        /// <summary>
        /// Calculates final results for one field. Results will be assigned to view items
        /// </summary>
        private void CalculateFinalResultsForField(
            List<CropViewItem> viewItemsForField,
            Farm farm,
            Guid fieldSystemGuid)
        {
            var fieldSystemComponent = farm.GetFieldSystemComponent(fieldSystemGuid);

            // Need to get leftmost component here
            var leftMost = this.GetLeftMostComponent(fieldSystemComponent!, farm)!;

            // Create run in period items
            var runInPeriodItems = this.GetRunInPeriodItems(farm, leftMost.CropViewItems, leftMost.StartYear,
                viewItemsForField, leftMost);

            _cropInitializationService.InitializeYieldForAllYears(runInPeriodItems, farm, leftMost);

            // CLI-loaded farms may have residue-input prerequisites the user left at zero; fill them
            // before the carbon model runs. No-op in GUI mode (inputs already assigned upstream).
            this.ProcessCommandLineItems(viewItemsForField, farm);

            // Check if user specified ICBM or Tier 2 carbon modelling
            if (farm.Defaults.CarbonModellingStrategy == CarbonModellingStrategies.IPCCTier2)
            {
                _tier2SoilCarbonCalculator.AnimalComponentEmissionsResults = this.AnimalResults;
                _tier2SoilCarbonCalculator.N2OEmissionFactorCalculator.DigestateService.AnimalResults =
                    this.AnimalResults;
                _tier2SoilCarbonCalculator.N2OEmissionFactorCalculator.ManureService.Initialize(farm,
                    this.AnimalResults);

                foreach (var runInPeriodItem in runInPeriodItems)
                {
                    if (_tier2SoilCarbonCalculator.CanCalculateInputsForCrop(runInPeriodItem))
                    {
                        _tier2SoilCarbonCalculator.CalculateInputs(runInPeriodItem, farm);
                    }
                    else
                    {
                        _icbmCarbonInputCalculator.AssignInputs(null!, runInPeriodItem, null!, farm, this.AnimalResults);
                    }
                }

                // Combine inputs now that we have C set on cover crops
                this.CombineInputsForAllCropsInSameYear(runInPeriodItems, leftMost);

                // Merge all run in period items
                var mergedRunInItems = this.MergeDetailViewItems(runInPeriodItems, leftMost);

                // Combine inputs for run in period
                this.CombineInputsForAllCropsInSameYear(mergedRunInItems, leftMost);

                _tier2SoilCarbonCalculator.CalculateResults(
                    farm: farm,
                    viewItemsByField: viewItemsForField,
                    fieldSystemComponent: leftMost,
                    runInPeriodItems: mergedRunInItems);
            }
            else
            {
                _icbmSoilCarbonCalculator.AnimalComponentEmissionsResults = this.AnimalResults;
                _icbmSoilCarbonCalculator.N2OEmissionFactorCalculator.DigestateService.AnimalResults =
                    this.AnimalResults;
                _icbmSoilCarbonCalculator.N2OEmissionFactorCalculator.ManureService
                    .Initialize(farm, this.AnimalResults);

                // Create the item with the steady-state (equilibrium) values. Note: if the climate
                // parameter is ~0 the equilibrium denominator can produce NaN/Infinity (and the chart
                // then drops the series) — see the failure-mode table in Carbon_Model_Flow.md.
                var equilibriumYearResults = _icbmSoilCarbonCalculator.CalculateEquilibriumYear(viewItemsForField, farm, fieldSystemGuid);

                for (int i = 0; i < viewItemsForField.Count; i++)
                {
                    // List<T> indexer is O(1); the previous ElementAt(int) here dispatched through
                    // the IEnumerable<T> extension every iteration. Minor on its own, but the loop
                    // runs per (field × year).
                    var currentYearResults = viewItemsForField[i];

                    // Get previous year results, if there is no previous year (i.e. t = 0), then use equilibrium (or custom measured) values for the pools
                    var previousYearResults = i == 0 ? equilibriumYearResults : viewItemsForField[i - 1];

                    // Carbon must be calculated before nitrogen
                    _icbmSoilCarbonCalculator.CalculateCarbonAtInterval(
                        previousYearResults: previousYearResults,
                        currentYearResults: currentYearResults,
                        farm: farm);

                    _icbmSoilCarbonCalculator.CalculateNitrogenAtInterval(
                        previousYearResults: previousYearResults,
                        currentYearResults: currentYearResults,
                        nextYearResults: null,
                        farm: farm,
                        yearIndex: i);
                }
            }

            foreach (var cropViewItem in viewItemsForField)
            {
                var energyResults = this.CalculateCropEnergyResults(cropViewItem, farm);
                cropViewItem.CropEnergyResults = energyResults;
                cropViewItem.EstimatesOfProductionResultsViewItem =
                    this.CalculateEstimateOfProduction(cropViewItem, fieldSystemComponent!);
            }
        }

        /// <summary>
        /// Calculates how much carbon was lost due to bales being exported off field.
        /// </summary>
        public void CalculateCarbonLostFromHayExports(
            FieldSystemComponent fieldSystemComponent,
            Farm farm)
        {
            // Get hay exports from component, find other components dependent on exports, assign losses to view item
            foreach (var cropViewItem in fieldSystemComponent.CropViewItems)
            {
                if (cropViewItem.HasHarvestViewItems)
                {
                    var totalHarvestedBales = cropViewItem.HarvestViewItems.Sum(x => x.TotalNumberOfBalesHarvested);
                    var totalBalesImportedFromOtherFields = 0d;
                    var moistureContentOfImportedBales = new List<double>();

                    // Check all other fields to see if anyone is using these harvested bales
                    foreach (var component in farm.FieldSystemComponents)
                    {
                        foreach (var viewItem in component.CropViewItems)
                        {
                            foreach (var hayImportViewItem in viewItem.HayImportViewItems.Where(x =>
                                         x.FieldSourceGuid.Equals(fieldSystemComponent.Guid) &&
                                         x.Date.Year == cropViewItem.Year))
                            {
                                if (hayImportViewItem.SourceOfBales == ResourceSourceLocation.OffFarm)
                                {
                                    continue;
                                }

                                totalBalesImportedFromOtherFields += hayImportViewItem.NumberOfBales;
                                moistureContentOfImportedBales.Add(hayImportViewItem.MoistureContentAsPercentage);
                            }
                        }
                    }

                    if (totalBalesImportedFromOtherFields == 0)
                    {
                        continue;
                    }

                    var totalWetWeight = totalHarvestedBales - totalBalesImportedFromOtherFields;
                    if (totalWetWeight > 0)
                    {
                        // Get average moisture content to calculate the dry matter
                        var averageMoistureContentPercentage = moistureContentOfImportedBales.Average();

                        // Calculate dry matter of the bales
                        var totalDryMatter = totalWetWeight * (1 - (averageMoistureContentPercentage / 100.0));

                        // Calculate total carbon in dry matter
                        var totalCarbonInExportedBales = totalDryMatter * CoreConstants.CarbonConcentration;

                        cropViewItem.TotalCarbonLossFromBaleExports = this.CalculateTotalCarbonLossFromBaleExports(
                            percentageOfProductReturnedToSoil: cropViewItem.PercentageOfProductYieldReturnedToSoil,
                            totalCarbonInExportedBales: totalCarbonInExportedBales);
                    }
                }
            }
        }

        /// <summary>
        /// Equation 12.3.2-4
        /// </summary>
        /// <param name="percentageOfProductReturnedToSoil">Amount of product yield returned to soil (%)</param>
        /// <param name="totalCarbonInExportedBales">Total amount of carbon in bales (kg C)</param>
        /// <returns>Total amount of carbon lost from exported bales (kg C)</returns>
        private double CalculateTotalCarbonLossFromBaleExports(
            double percentageOfProductReturnedToSoil,
            double totalCarbonInExportedBales)
        {
            var result = totalCarbonInExportedBales / (1 - (percentageOfProductReturnedToSoil / 100.0));

            return result;
        }

        /// <summary>
        /// Calculates how much carbon added from manure of animals grazing on the field.
        /// </summary>
        public void CalculateManureCarbonInputByGrazingAnimals(
            FieldSystemComponent fieldSystemComponent,
            List<CropViewItem> cropViewItems)
        {
            this.CalculateManureCarbonInputByGrazingAnimals(
                fieldSystemComponent: fieldSystemComponent,
                results: this.AnimalResults,
                cropViewItems);
        }

        /// <summary>
        /// Equation 5.6.1-1
        ///
        /// (kg C ha^-1)
        /// </summary>
        public void CalculateManureCarbonInputByGrazingAnimals(FieldSystemComponent fieldSystemComponent,
            IEnumerable<AnimalComponentEmissionsResults> results, List<CropViewItem> cropViewItems)
        {
            foreach (var cropViewItem in cropViewItems)
            {
                cropViewItem.TotalCarbonInputFromManureFromAnimalsGrazingOnPasture =
                    this.CalculateManureCarbonInputFromGrazingAnimals(fieldSystemComponent, cropViewItem, results.ToList());
            }
        }

        /// <summary>
        /// (kg C ha^-1)
        /// </summary>
        public double CalculateManureCarbonInputFromGrazingAnimals(
            FieldSystemComponent fieldSystemComponent,
            CropViewItem cropViewItem,
            List<AnimalComponentEmissionsResults> results)
        {
            var result = 0d;

            var grazingViewItems = fieldSystemComponent.CropViewItems.Where(y => y.CropType == cropViewItem.CropType).SelectMany(x => x.GrazingViewItems).ToList();

            var grazingItems = grazingViewItems.Where(x => x.Start.Year == cropViewItem.Year).ToList();

            foreach (var grazingViewItem in grazingItems)
            {
                var emissionsFromGrazingAnimals = this.GetGroupEmissionsFromGrazingAnimals(results, grazingViewItem).ToList();
                foreach (var groupEmissionsByMonth in emissionsFromGrazingAnimals.ToList())
                {
                    result += (groupEmissionsByMonth.MonthlyFecalCarbonExcretion -
                               groupEmissionsByMonth.MonthlyManureMethaneEmission) / cropViewItem.Area;
                }
            }

            return result < 0 ? 0 : result;
        }

        /// <summary>
        /// Equation 11.3.2-4
        /// </summary>
        public void CalculateCarbonLostByGrazingAnimals(Farm farm,
            FieldSystemComponent fieldSystemComponent,
            IEnumerable<AnimalComponentEmissionsResults> animalComponentEmissionsResults, List<CropViewItem> viewItems)
        {
            foreach (var cropViewItem in viewItems)
            {
                if (cropViewItem.HarvestMethod == HarvestMethods.StubbleGrazing)
                {
                    continue;
                }

                var totalCarbonLossesForField = 0d;
                var totalCarbonUptakeForField = 0d;

                foreach (var componentResults in animalComponentEmissionsResults.ToList())
                {
                    if (componentResults.Component is AnimalComponentBase animalComponentBase)
                    {
                        var totalLostForAllGroups = 0d;
                        var totalCarbonUptakeForAllGroups = 0d;

                        var animalGroups = animalComponentBase.Groups;
                        foreach (var animalGroup in animalGroups)
                        {
                            // managementPeriod.Start.Year == currentYearResults.Year
                            var grazingManagementPeriodsByGroup = this.AnimalResultsService.GetGrazingManagementPeriods(animalGroup, fieldSystemComponent).Where(x => x.Start.Year == cropViewItem.Year).ToList();
                            var totalCarbonLostForAllManagementPeriods = this.CalculateUptakeByGrazingAnimals(grazingManagementPeriodsByGroup, cropViewItem, animalGroup, fieldSystemComponent, farm, animalComponentBase);

                            totalLostForAllGroups += totalCarbonLostForAllManagementPeriods.Item1;
                            totalCarbonUptakeForAllGroups += totalCarbonLostForAllManagementPeriods.Item2;
                        }

                        totalCarbonLossesForField += totalLostForAllGroups;
                        totalCarbonUptakeForField += totalCarbonUptakeForAllGroups;
                    }
                }

                cropViewItem.TotalCarbonLossesByGrazingAnimals = totalCarbonLossesForField;
                cropViewItem.TotalCarbonUptakeByAnimals = totalCarbonUptakeForField;
            }
        }

        public Tuple<double, double> CalculateUptakeByGrazingAnimals(List<ManagementPeriod> managementPeriods, CropViewItem viewItem, AnimalGroup animalGroup, FieldSystemComponent fieldSystemComponent, Farm farm, AnimalComponentBase animalComponent)
        {
            if (managementPeriods.Any() == false)
            {
                return new Tuple<double, double>(0, 0);
            }

            if (managementPeriods.Count == 1)
            {
                // Equation 11.3.2-4
                // Equation 11.3.2-5

                /*
                 * Note: value is reduced by area in Equation 11.3.2-9
                 */

                var resultsForManagementPeriod = this.AnimalResultsService.GetResultsForManagementPeriod(animalGroup, farm, animalComponent, managementPeriods.Single());
                var totalCarbonUptake = resultsForManagementPeriod.TotalCarbonUptakeByAnimals();
                var averageUtilizationRate =  viewItem.GrazingViewItems.Any() ? viewItem.GrazingViewItems.Average(x => x.Utilization) : 0;
                var utilizationRate = (averageUtilizationRate / 100.0);
                if (utilizationRate <= 0)
                {
                    utilizationRate = 1;
                }

                var result = totalCarbonUptake / utilizationRate;

                return new Tuple<double, double>(result, totalCarbonUptake);
            }
            else
            {
                var totalCarbonUptake = 0d;
                var averageUtilizationRate = viewItem.GrazingViewItems.Any() ? viewItem.GrazingViewItems.Average(x => x.Utilization) : 0;

                var lastPeriod = managementPeriods.OrderBy(x => x.Start).Last();
                var resultsForLastManagementPeriod = this.AnimalResultsService.GetResultsForManagementPeriod(animalGroup, farm, animalComponent, lastPeriod);
                var totalCarbonUptakeForLastPeriod = resultsForLastManagementPeriod.TotalCarbonUptakeByAnimals();
                totalCarbonUptake += totalCarbonUptakeForLastPeriod;
                var denominator = averageUtilizationRate / 100;
                if (denominator <= 0)
                {
                    denominator = 1;
                }

                var carbonUptakeForLastPeriod = totalCarbonUptakeForLastPeriod / denominator;

                // Equation 11.3.2-6
                var carbonUpdateForOtherPeriods = 0d;
                for (int i = 0; i < managementPeriods.Count - 1; i++)
                {
                    var managementPeriod = managementPeriods[i];
                    var resultsForPeriod = this.AnimalResultsService.GetResultsForManagementPeriod(animalGroup, farm, animalComponent, managementPeriod);
                    var totalCarbonUptakeForPeriod = resultsForPeriod.TotalCarbonUptakeByAnimals();
                    totalCarbonUptake += totalCarbonUptakeForPeriod;

                    var carbonUptakeForPeriod = totalCarbonUptakeForPeriod ;
                    carbonUpdateForOtherPeriods += carbonUptakeForPeriod;
                }

                var result = (carbonUpdateForOtherPeriods + carbonUptakeForLastPeriod);

                return new Tuple<double, double>(result, totalCarbonUptake);
            }
        }

        #endregion
    }
}