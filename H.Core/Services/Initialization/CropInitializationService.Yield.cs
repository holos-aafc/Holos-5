using H.Core.Enumerations;
using H.Core.Models;
using H.Core.Models.LandManagement.Fields;
using H.Core.Providers.Plants;
using H.Core.Providers.Soil;
using H.Infrastructure;
using Microsoft.Extensions.Logging;

namespace H.Core.Services.Initialization;

public partial class CropInitializationService
{
    #region Public Methods

    public void InitializeYield(CropViewItem viewItem, Farm farm)
    {
        if (viewItem.CropType.IsSilageCropWithoutDefaults())
        {
            this.InitializeDefaultSilageCropYield(viewItem, farm);
        }
        else
        {
            this.InitializeDefaultYield(viewItem, farm);
        }
    }

    /// <summary>
    /// Assigns a default yield to the view item using the default (small-area-data) yield provider.
    /// </summary>
    public void InitializeDefaultYield(CropViewItem viewItem, Farm farm)
    {
        var province = farm.Province;

        // No small area data exists for years > 2018, take average of last 10 years as placeholder values when considering these years
        const int NoDataYear = 2018;
        const int NumberOfYearsInAverage = 10;
        if (viewItem.Year > NoDataYear)
        {
            var startYear = NoDataYear - NumberOfYearsInAverage;
            var yields = new List<double>();
            for (int year = startYear; year <= NoDataYear; year++)
            {
                var smallAreaYieldData = _smallAreaYieldProvider.GetData(
                    year: year,
                    polygon: farm.PolygonId,
                    cropType: viewItem.CropType,
                    province: province);

                if (smallAreaYieldData != null)
                {
                    yields.Add(smallAreaYieldData.Yield);
                }
            }

            if (yields.Any())
            {
                viewItem.Yield = Math.Round(yields.Average(), 1);
                viewItem.DefaultYield = viewItem.Yield;
            }
            else
            {
                _logger.LogWarning("No default yield data found for {CropType}", viewItem.CropType.GetDescription());
            }

            viewItem.CalculateDryYield();

            return;
        }

        var smallAreaYield = _smallAreaYieldProvider.GetData(
            year: viewItem.Year,
            polygon: farm.PolygonId,
            cropType: viewItem.CropType,
            province: province);

        if (smallAreaYield != null)
        {
            viewItem.Yield = smallAreaYield.Yield;
            viewItem.CalculateDryYield();
        }
        else
        {
            _logger.LogWarning("No default yield data found for {CropType} in {Year}", viewItem.CropType.GetDescription(), viewItem.Year);
        }
    }

    public void InitializeYieldForAllYears(IEnumerable<CropViewItem> cropViewItems, Farm farm,
        FieldSystemComponent fieldSystemComponent)
    {
        foreach (var viewItem in cropViewItems)
        {
            this.InitializeYieldForYear(farm, viewItem, fieldSystemComponent);
        }
    }

    /// <summary>
    /// Assigns a yield to one view item for a field according to the farm's (or field's) yield
    /// assignment method.
    /// </summary>
    public void InitializeYieldForYear(
        Farm farm,
        CropViewItem viewItem,
        FieldSystemComponent fieldSystemComponent)
    {
        var yieldAssignmentMethod = farm.UseFieldLevelYieldAssignement ? fieldSystemComponent.YieldAssignmentMethod : farm.YieldAssignmentMethod;
        if (viewItem.CropType == CropType.NotSelected || viewItem.Year == 0)
        {
            _logger.LogError("{Service}.{Method}: bad crop type or bad year for view item '{ViewItem}'",
                nameof(CropInitializationService), nameof(InitializeYieldForYear), viewItem);

            viewItem.Yield = 0;
        }

        /*
         * The user will enter (or has entered) yields for each year manually, do not overwrite the value
         */

        if (yieldAssignmentMethod == YieldAssignmentMethod.Custom)
        {
            return;
        }

        /*
         * Use an average of the crops
         */

        if (yieldAssignmentMethod == YieldAssignmentMethod.Average)
        {
            // Create an average from the crops that define the rotation
            var average = fieldSystemComponent.CropViewItems.Average(cropViewItem => cropViewItem.Yield);

            viewItem.Yield = average;

            return;
        }

        if (yieldAssignmentMethod == YieldAssignmentMethod.SmallAreaData ||
            yieldAssignmentMethod == YieldAssignmentMethod.CARValue)
        {
            // If the cropviewitem is of a silage crop, we assign defaults using a different method.
            if (viewItem.CropType.IsSilageCropWithoutDefaults())
            {
                this.InitializeDefaultSilageCropYield(viewItem, farm);
            }
            else
            {
                this.InitializeDefaultYield(viewItem, farm);
            }

            return;
        }

        /*
         * Use values from a custom yield file whose path has been specified by the user
         */

        if (yieldAssignmentMethod == YieldAssignmentMethod.InputFile)
        {
            var results = new List<CustomUserYieldData>();

            foreach (var customYieldItem in farm.GeographicData.CustomYieldData)
            {
                var yearMatch = customYieldItem.Year == viewItem.Year;
                var fieldNameMatch = (fieldSystemComponent.Name ?? string.Empty).IndexOf(customYieldItem.FieldName.Trim(), StringComparison.InvariantCultureIgnoreCase) >= 0;
                var farmNameMatch = (farm.Name ?? string.Empty).IndexOf(customYieldItem.RotationName.Trim(), StringComparison.InvariantCultureIgnoreCase) >= 0;


                // Don't assign main year yields to a cover crop yield (for now)
                if (yearMatch && fieldNameMatch && farmNameMatch && viewItem.IsSecondaryCrop == false)
                {
                    results.Add(customYieldItem);
                }
            }

            CustomUserYieldData? yieldDataRow = null;
            if (results.Count > 1)
            {
                yieldDataRow = results.FirstOrDefault(x => x.FieldName.Trim().Equals(fieldSystemComponent.Name?.Trim(), StringComparison.InvariantCultureIgnoreCase));
            }
            else if (results.Count == 1)
            {
                yieldDataRow = results.Single();
            }

            if (yieldDataRow != null)
            {
                viewItem.Yield = yieldDataRow.Yield;
            }
            else
            {
                _logger.LogError("{Service}.{Method}: no custom yield data for {Year} and {Field} was found in custom yield file. Attempting to assign a default yield for this year from the default yield provider.",
                    nameof(CropInitializationService), nameof(InitializeYieldForYear), viewItem.Year, fieldSystemComponent.Name);

                // With the Tier 2 model, we need to have yields for the run-in years. If the user loads a custom yield file, they might not have yields for this period. In this case,
                // we check if we can get yields for these years by checking the small area data table.
                this.InitializeDefaultYield(
                    viewItem: viewItem,
                    farm: farm);

                if (viewItem.Yield == 0)
                {
                    _logger.LogError("{Service}.{Method}: no yield data for {Year} and {Field} was found.",
                        nameof(CropInitializationService), nameof(InitializeYieldForYear), viewItem.Year, fieldSystemComponent.Name);
                }
            }

            return;
        }

        throw new Exception("Yield assignment method not accounted for");
    }

    /// <summary>
    /// Calculates the yield of a silage crop using information from the grain crop equivalent to that
    /// silage crop e.g. if silage crop is Barley Silage, its grain equivalent will be Barley.
    /// </summary>
    public void InitializeDefaultSilageCropYield(CropViewItem silageCropViewItem, Farm farm)
    {
        // Find the grain crop equivalent of the silage crop.
        var grainCrop = silageCropViewItem.CropType.GetGrainCropEquivalentOfSilageCrop();

        // Create a new CropViewItem that will represent this grain crop. It gets assigned the same year as the silage crop.
        var grainCropViewItem = new CropViewItem
        {
            Year = silageCropViewItem.Year,
            CropType = grainCrop,
        };
        // We call InitializeCrop with the CropViewItem representing the grain crop to get its default values.
        var globalSettings = new GlobalSettings();
        this.InitializeCrop(grainCropViewItem, farm, globalSettings);

        // We specifically find the PlantCarbonInAgriculturalProduct of the grain crop as that is needed in the yield calculation.
        grainCropViewItem.PlantCarbonInAgriculturalProduct = _icbmCarbonInputCalculator.CalculatePlantCarbonInAgriculturalProduct(previousYearViewItem: null!, currentYearViewItem: grainCropViewItem, farm: farm);

        // We then calculate the wet and dry yield of the crop.
        silageCropViewItem.DryYield = CalculateSilageCropYield(grainCropViewItem: grainCropViewItem);
        silageCropViewItem.CalculateWetWeightYield();

        // No defaults for any grass silages so we use SAD data
        if (silageCropViewItem.CropType == CropType.GrassSilage)
        {
            this.InitializeDefaultYield(silageCropViewItem, farm);
            silageCropViewItem.CalculateWetWeightYield();
        }
    }

    /// <summary>
    /// Equation 2.1.2-13
    ///
    /// Calculates the default yield for a silage crop using information from its grain crop equivalent.
    /// </summary>
    public double CalculateSilageCropYield(CropViewItem grainCropViewItem)
    {
        if (grainCropViewItem.BiomassCoefficientProduct == 0)
        {
            return 0;
        }

        var term1 = grainCropViewItem.Yield + grainCropViewItem.Yield * (grainCropViewItem.BiomassCoefficientStraw / grainCropViewItem.BiomassCoefficientProduct);
        var term2 = term1 * (1 + (grainCropViewItem.PercentageOfProductYieldReturnedToSoil / 100.0));
        var result = term2 * (1 - grainCropViewItem.MoistureContentOfCrop);

        return result;
    }

    #endregion
}
