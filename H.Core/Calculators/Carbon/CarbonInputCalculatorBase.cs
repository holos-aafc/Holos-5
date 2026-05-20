using H.Core.Emissions.Results;
using H.Core.Models;
using H.Core.Models.LandManagement.Fields;
using H.Core.Services.Animals;

namespace H.Core.Calculators.Carbon;

/// <summary>
/// Shared base for the two carbon-input calculators (<see cref="ICBMCarbonInputCalculator"/>
/// and <see cref="IPCCTier2CarbonInputCalculator"/>). Owns the parts of the input math both
/// strategies share — most notably the supplemental-hay-to-grazing-animals contribution
/// (equations 2.1.2-34 / 2.1.2-2) which feeds <c>AboveGroundCarbonInput</c> in both models.
///
/// <para><b>Dependencies are injected:</b></para>
/// The manure / digestate / animal services are supplied via the constructor so the container
/// hands every carbon calculator the same registered singletons (rather than each calculator
/// newing its own). Subclasses chain to this base ctor.
/// </summary>
public class CarbonInputCalculatorBase : ICarbonInputCalculator
{
    #region Fields

    /// <summary>Manure C / N source — initialized per-farm by <c>AssignInputs</c> before each calc.</summary>
    protected readonly IManureService manureService;

    /// <summary>Digestate C / N source — same lifecycle as <see cref="manureService"/>.</summary>
    protected readonly IDigestateService digestateService;

    /// <summary>Animal-pipeline accessor for grazing / supplemental-feed lookups.</summary>
    protected readonly IAnimalService animalService;

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor injection for the manure / digestate / animal services. Subclasses inherit the
    /// instances; callers re-initialize them per-farm before each carbon-input pass.
    /// </summary>
    public CarbonInputCalculatorBase(
        IManureService manureService,
        IDigestateService digestateService,
        IAnimalService animalService)
    {
        this.manureService = manureService ?? throw new ArgumentNullException(nameof(manureService));
        this.digestateService = digestateService ?? throw new ArgumentNullException(nameof(digestateService));
        this.animalService = animalService ?? throw new ArgumentNullException(nameof(animalService));
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Equation 2.1.2-34
    /// Equation 2.1.2-2
    ///
    /// (kg C ha^-1)
    /// </summary>
    public double CalculateInputsFromSupplementalHayFedToGrazingAnimals(
        CropViewItem previousYearViewItem,
        CropViewItem currentYearViewItem,
        CropViewItem nextYearViewItems,
        Farm farm)
    {
        var result = 0.0;

        foreach (var hayImportViewItem in currentYearViewItem.HayImportViewItems)
        {
            var totalDryMatterWeight = hayImportViewItem.GetTotalDryMatterWeightOfAllBales();
            var loss = farm.Defaults.DefaultSupplementalFeedingLossPercentage / 100;

            // Moisture content is already considered in GetTotalDryMatterWeightOfAllBales, so it
            // is not included here as it is in the equation from the algorithm document.
            var totalCarbon = totalDryMatterWeight * loss * currentYearViewItem.CarbonConcentration;

            result += totalCarbon;
        }

        return result / currentYearViewItem.Area;
    }

    /// <summary>
    /// Equation 11.3.2-2 (b)
    ///
    /// (kg C)
    /// </summary>
    public double GetSupplementalLosses(
        CropViewItem previousYearViewItem,
        CropViewItem currentYearViewItem,
        CropViewItem nextYearViewItems,
        Farm farm)
    {
        var result = 0.0;

        foreach (var hayImportViewItem in currentYearViewItem.HayImportViewItems)
        {
            var totalDryMatterWeight = hayImportViewItem.GetTotalDryMatterWeightOfAllBales();
            var totalCarbon = totalDryMatterWeight * currentYearViewItem.CarbonConcentration;
            var loss = farm.Defaults.DefaultSupplementalFeedingLossPercentage / 100;

            result += (totalCarbon / loss) * (1 - loss);
        }

        return result;
    }

    public void AssignManureCarbonInputs(CropViewItem viewItem, Farm farm, List<AnimalComponentEmissionsResults> animalComponentEmissionsResults)
    {
        manureService.Initialize(farm, animalComponentEmissionsResults);
        digestateService.Initialize(farm, animalComponentEmissionsResults);

        if (farm.IsCommandLineMode == false)
        {
            viewItem.ManureCarbonInputsPerHectare = manureService.GetTotalManureCarbonInputsForField(farm, viewItem.Year, viewItem);
            viewItem.ManureCarbonInputsFromManureOnly = viewItem.GetTotalCarbonFromAppliedManure() / viewItem.Area;
        }
        else if (!viewItem.IsRunInPeriodItem && viewItem.ManureCarbonInputsPerHectare <= 0)
        {
            // CLI: if the user did not specify a non-zero manure C input in the field input file,
            // recalculate on their behalf since they may have a manure application but no
            // total C value. Run-in-period items are skipped so their totals stay consistent
            // with the GUI's run-in totals. If the user supplied a non-zero value, leave it alone.
            viewItem.ManureCarbonInputsPerHectare = manureService.GetTotalManureCarbonInputsForField(farm, viewItem.Year, viewItem);
            viewItem.ManureCarbonInputsFromManureOnly = viewItem.GetTotalCarbonFromAppliedManure() / viewItem.Area;
        }

        viewItem.ManureCarbonInputsPerHectare += viewItem.TotalCarbonInputFromManureFromAnimalsGrazingOnPasture;
    }

    #endregion
}
