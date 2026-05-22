#region Imports

using H.Core.Calculators.Nitrogen;
using H.Core.Enumerations;
using H.Core.Models;
using H.Core.Models.LandManagement.Fields;
using H.Core.Providers.Climate;
using H.Core.Services.LandManagement;

#endregion

namespace H.Core.Calculators.Carbon
{
    /// <summary>
    /// Implementation of the ICBM (Introductory Carbon Balance Model) soil-carbon calculator —
    /// Holos' default carbon model. ICBM splits soil organic carbon into four pools that decay
    /// exponentially:
    /// <list type="bullet">
    ///   <item><b>Young pool — above-ground (Y_ag):</b> fresh above-ground residue from crop straw + cover crops.</item>
    ///   <item><b>Young pool — below-ground (Y_bg):</b> root and extra-root carbon.</item>
    ///   <item><b>Young pool — manure (Y_manure):</b> manure / digestate inputs treated as a separate young pool.</item>
    ///   <item><b>Old pool (O):</b> stabilized humified C, decays much more slowly.</item>
    /// </list>
    ///
    /// <para><b>Responsibility — pool dynamics + nitrogen only:</b></para>
    /// <c>CalculateYoungPoolSteadyState*</c> / <c>CalculateOldPoolSteadyState</c> /
    /// <c>CalculateYoungPool*AtInterval</c> / <c>CalculateOldPoolSoilCarbonAtInterval</c> /
    /// <c>CalculateSoilCarbonAtInterval</c> step the four pools, and the partial
    /// <c>ICBMSoilCarbonCalculator.Nitrogen.cs</c> carries the per-interval N math. Called from
    /// <c>FieldResultsService.CalculateFinalResultsForField</c> during the final pass: first to
    /// seed the equilibrium values, then once per year to step each pool.
    ///
    /// <para><b>Crop-carbon-input math lives elsewhere:</b></para>
    /// The per-crop, per-year input math (plant-C-in-product, residue fractions, manure /
    /// digestate C) lives on <see cref="ICBMCarbonInputCalculator"/> (Eq. 2.1.2-x), reached via
    /// <c>CarbonService</c> / <c>FieldResultsService</c>, so the input math has a single home.
    ///
    /// <para><b>Failure mode to watch for:</b></para>
    /// Every steady-state and interval equation has the form
    /// <c>numerator / (1 - exp(-k * climateParameter))</c>. A <c>climateParameter</c> of 0 makes
    /// the denominator 0 and produces <c>±Infinity</c> or <c>NaN</c>. The downstream chart in
    /// <c>GHGResultsView</c> silently drops NaN points, so a corrupted climate parameter looks
    /// like "the chart is empty". See the failure-mode table in <c>Carbon_Model_Flow.md</c>.
    ///
    /// <para>
    /// The nitrogen-side math lives in the partial class file
    /// <c>ICBMSoilCarbonCalculator.Nitrogen.cs</c>.
    /// </para>
    /// </summary>
    public partial class ICBMSoilCarbonCalculator : CarbonCalculatorBase, IICBMSoilCarbonCalculator
    {
        #region Fields


        #endregion

        #region Constructors

        /// <summary>
        /// DI constructor. The climate provider is used by the nitrogen-side math
        /// (precipitation / leaching factors); the N₂O factor calculator owns the manure /
        /// digestate / synthetic-fertilizer emission-factor logic that the per-year nitrogen
        /// step calls into.
        /// </summary>
        /// <exception cref="ArgumentNullException">If either dependency is <c>null</c>.</exception>
        public ICBMSoilCarbonCalculator(IClimateProvider climateProvider, N2OEmissionFactorCalculator n2OEmissionFactorCalculator)
        {
            _climateProvider = climateProvider ?? throw new ArgumentNullException(nameof(climateProvider));

            base.N2OEmissionFactorCalculator = n2OEmissionFactorCalculator ?? throw new ArgumentNullException(nameof(n2OEmissionFactorCalculator));
        }

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        /// <summary>
        /// Equation 2.2.2-27
        /// </summary>
        public double CalculateAmountOfNitrogenAppliedFromManure(
            double manureAmount, 
            double fractionOfNitrogenInAppliedManure)
        {
            return manureAmount * fractionOfNitrogenInAppliedManure;
        }

        /// <summary>
        /// Equation 2.2.2-28
        /// </summary>
        public double CalculateAmountOfPhosphorusAppliedFromManure(
            double manureAmount, 
            double fractionOfPhosphorusInAppliedManure)
        {
            return manureAmount * fractionOfPhosphorusInAppliedManure;
        }

        /// <summary>
        /// Equation 2.2.2-29
        /// </summary>
        public double CalculateMoistureOfManure(
            double manureAmount, 
            double waterFraction)
        {
            return manureAmount * waterFraction / 10000;
        }

        /// <summary>
        /// Equation 2.1.3-1
        /// </summary>
        public double CalculateAverageAboveGroundResidueCarbonInput(
            double carbonInputFromProductOfEachRotationPhase, 
            double carbonInputFromStrawOfEachRotationPhase)
        {
            return carbonInputFromProductOfEachRotationPhase + carbonInputFromStrawOfEachRotationPhase;
        }

        /// <summary>
        /// Equation 2.1.3-2
        /// </summary>
        public double CalculateAverageBelowGroundResidueCarbonInput(
            double carbonInputFromRootsOfEachRotationPhase, 
            double carbonInputFromExtrarootOfEachRotationPhase)
        {
            return carbonInputFromExtrarootOfEachRotationPhase + carbonInputFromRootsOfEachRotationPhase;
        }

        /// <summary>
        /// Equation 2.1.3-3
        /// </summary>
        public double CalculateAverageManureCarbonInput(double carbonInputsFromManureInputsOfEachRotationPhase)
        {
            return carbonInputsFromManureInputsOfEachRotationPhase;
        }

        /// <summary>
        /// Equation 2.1.3-4. Steady-state value of the above-ground young pool: how much C this
        /// pool holds when input and decay are balanced over the rotation. Seeds
        /// <c>YoungPoolSoilCarbonAboveGround</c> on the equilibrium-year view item that
        /// <see cref="CalculateEquilibriumYear"/> builds.
        ///
        /// <para><b>NaN failure mode:</b></para>
        /// The denominator is <c>1 - exp(-k * climateParameter)</c>. When <c>climateParameter</c>
        /// is 0 the denominator is 0 and the result is <c>±Infinity</c> or <c>NaN</c>, which
        /// propagates through <c>CalculateSoilCarbonAtInterval</c> to the chart and produces the
        /// "empty chart" symptom. That used to fire on non-Canadian farms whose SLC climate
        /// lookup returned 0 — now blocked by the province guard in <c>FarmAnalysisService</c>.
        /// </summary>
        public double CalculateYoungPoolSteadyStateAboveGround(
            double averageAboveGroundCarbonInput,
            double youngPoolDecompositionRate,
            double climateParameter)
        {
            var numerator = averageAboveGroundCarbonInput * Math.Exp(-1 * youngPoolDecompositionRate * climateParameter);
            var denominator = 1 - Math.Exp(-1 * youngPoolDecompositionRate * climateParameter);

            var result = numerator / denominator;

            return result;
        }

        /// <summary>
        /// Equation 2.1.3-5
        /// </summary>
        public double CalculateYoungPoolSteadyStateBelowGround(
            double averageBelowGroundCarbonInput, 
            double youngPoolDecompositionRate, 
            double climateParameter)
        {
            var numerator = averageBelowGroundCarbonInput * Math.Exp(-1 * youngPoolDecompositionRate * climateParameter);
            var denominator = 1 - Math.Exp(-1 * youngPoolDecompositionRate * climateParameter);

            var result = numerator / denominator;

            return result;
        }

        /// <summary>
        /// Equation 2.1.3-6
        /// </summary>
        public double CalculateYoungPoolSteadyStateManure(
            double averageManureCarbonInput, 
            double youngPoolDecompositionRate, 
            double climateParameter)
        {
            var numerator = averageManureCarbonInput * Math.Exp(-1 * youngPoolDecompositionRate * climateParameter);
            var denominator = 1 - Math.Exp(-1 * youngPoolDecompositionRate * climateParameter);

            var result = numerator / denominator;

            return result;
        }

        /// <summary>
        /// Equation 2.1.3-7
        /// </summary>
        public double CalculateOldPoolSteadyState(
            double youngPoolDecompositionRate, 
            double oldPoolDecompositionRate, 
            double climateParameter,
            double aboveGroundHumificationCoefficient, 
            double belowGroundHumificationCoefficient, 
            double averageAboveGroundCarbonInputOfRotation, 
            double averageBelowGroundCarbonInputOfRotation, 
            double aboveGroundYoungPoolSteadyState, 
            double belowGroundYoungPoolSteadyState, 
            double manureYoungPoolSteadyState,
            double averageManureCarbonInputOfRotation, 
            double manureHumificationCoefficient)
        {
            var firstFactorNumerator = Math.Exp(-1 * youngPoolDecompositionRate * climateParameter) - Math.Exp(-1 * oldPoolDecompositionRate * climateParameter);
            var firstFactorDenominator = 1 - Math.Exp(-1 * oldPoolDecompositionRate * climateParameter);

            var secondFactorNumeratorFactorOne = aboveGroundHumificationCoefficient * youngPoolDecompositionRate;
            var secondFactorNumeratorFactorTwo = aboveGroundYoungPoolSteadyState + averageAboveGroundCarbonInputOfRotation;

            var secondFactorNumeratorFactorThree = belowGroundHumificationCoefficient * youngPoolDecompositionRate;
            var secondFactorNumeratorFactorFour = belowGroundYoungPoolSteadyState + averageBelowGroundCarbonInputOfRotation;

            var secondFactorNumeratorFactorFive = manureHumificationCoefficient * youngPoolDecompositionRate;
            var secondFactorNumeratorFactorSix = manureYoungPoolSteadyState + averageManureCarbonInputOfRotation;

            var secondFactorNumerator = secondFactorNumeratorFactorOne * secondFactorNumeratorFactorTwo +
                                        secondFactorNumeratorFactorThree * secondFactorNumeratorFactorFour +
                                        secondFactorNumeratorFactorFive * secondFactorNumeratorFactorSix;

            var secondFactorDenominator = oldPoolDecompositionRate - youngPoolDecompositionRate;

            var result = (firstFactorNumerator / firstFactorDenominator) * (secondFactorNumerator / secondFactorDenominator);

            return result;
        }


        /// <summary>
        /// Equation 2.1.3-11
        /// </summary>
        public double CalculateYoungPoolAboveGroundCarbonAtInterval(
            double youngPoolAboveGroundCarbonAtPreviousInterval, 
            double aboveGroundCarbonAtPreviousInterval, 
            double youngPoolDecompositionRate, 
            double climateParameter)
        {
            var firstFactor = youngPoolAboveGroundCarbonAtPreviousInterval + aboveGroundCarbonAtPreviousInterval;
            var secondFactor = Math.Exp(-1 * youngPoolDecompositionRate * climateParameter);

            var result = firstFactor * secondFactor;

            return result;
        }

        /// <summary>
        /// Equation 2.1.3-12
        /// </summary>
        public double CalculateYoungPoolBelowGroundCarbonAtInterval(
            double youngPoolBelowGroundCarbonAtPreviousInterval, 
            double belowGroundCarbonAtPreviousInterval, 
            double youngPoolDecompositionRate, 
            double climateParameter)
        {
            var firstFactor = youngPoolBelowGroundCarbonAtPreviousInterval + belowGroundCarbonAtPreviousInterval;
            var secondFactor = Math.Exp(-1 * youngPoolDecompositionRate * climateParameter);

            var result = firstFactor * secondFactor;

            return result;
        }

        /// <summary>
        /// Equation 2.1.3-13
        /// </summary>
        public double CalculateYoungPoolManureCarbonAtInterval(
            double youngPoolManureCarbonAtPreviousInterval, 
            double manureCarbonInputAtPreviousInterval,
            double youngPoolDecompositionRate, 
            double climateParameter)
        {
            var firstFactor = youngPoolManureCarbonAtPreviousInterval + manureCarbonInputAtPreviousInterval;
            var secondFactor = Math.Exp(-1 * youngPoolDecompositionRate * climateParameter);

            var result = firstFactor * secondFactor;

            return result;
        }

        /// <summary>
        /// Equation 2.1.3-14
        /// </summary>
        /// <returns></returns>
        public double CalculateOldPoolSoilCarbonAtInterval(
            double oldPoolSoilCarbonAtPreviousInterval, 
            double aboveGroundHumificationCoefficient, 
            double belowGroundHumificationCoefficient, 
            double youngPoolDecompositionRate, 
            double oldPoolDecompositionRate, 
            double youngPoolAboveGroundOrganicCarbonAtPreviousInterval, 
            double youngPoolBelowGroundOrganicCarbonAtPreviousInterval, 
            double aboveGroundCarbonResidueAtPreviousInterval, 
            double belowGroundCarbonResidueAtPreviousInterval, 
            double climateParameter, 
            double youngPoolManureAtPreviousInterval, 
            double manureHumificationCoefficient, 
            double manureCarbonInputAtPreviousInterval)
        {
            var decompositionRateDifference = oldPoolDecompositionRate - youngPoolDecompositionRate;

            var aboveGroundDivisionTermNumerator = youngPoolDecompositionRate *
                                                   (youngPoolAboveGroundOrganicCarbonAtPreviousInterval +
                                                    aboveGroundCarbonResidueAtPreviousInterval); 
            var aboveGroundDivisionTerm = aboveGroundHumificationCoefficient *
                                          (aboveGroundDivisionTermNumerator / decompositionRateDifference);

            var belowGroundDivisionTermNumerator = youngPoolDecompositionRate *
                                                   (youngPoolBelowGroundOrganicCarbonAtPreviousInterval +
                                                    belowGroundCarbonResidueAtPreviousInterval);
            var belowGroundDivisionTerm = belowGroundHumificationCoefficient *
                                          (belowGroundDivisionTermNumerator / decompositionRateDifference);

            var manureDivisionTermNumerator = youngPoolDecompositionRate *
                                              (youngPoolManureAtPreviousInterval + manureCarbonInputAtPreviousInterval);
            var manureDivisionTerm = manureHumificationCoefficient *
                                     (manureDivisionTermNumerator / decompositionRateDifference);

            var firstTerm =
                (oldPoolSoilCarbonAtPreviousInterval - aboveGroundDivisionTerm - belowGroundDivisionTerm -
                 manureDivisionTerm) * Math.Exp(-1 * oldPoolDecompositionRate * climateParameter);
            var secondTerm = aboveGroundDivisionTerm * Math.Exp(-1 * youngPoolDecompositionRate * climateParameter);
            var thirdTerm = belowGroundDivisionTerm * Math.Exp(-1 * youngPoolDecompositionRate * climateParameter);
            var fourthTerm = manureDivisionTerm * Math.Exp(-1 * youngPoolDecompositionRate * climateParameter);

            var result = firstTerm + secondTerm + thirdTerm + fourthTerm;

            return result;
        }

        /// <summary>
        /// Equation 2.1.3-15. Total soil organic carbon at the end of the interval is the sum of
        /// the four ICBM pools. This is the value that <see cref="CropViewItem.SoilCarbon"/> ends
        /// up holding and that the GUI chart reads.
        /// </summary>
        /// <returns>SoilCarbon (kg C ha⁻¹) — sum of the four pool inputs.</returns>
        public double CalculateSoilCarbonAtInterval(
            double youngPoolSoilCarbonAboveGroundAtInterval,
            double youngPoolSoilCarbonBelowGroundAtInterval,
            double oldPoolSoilCarbonAtInterval,
            double youngPoolManureAtInterval)
        {
            return youngPoolSoilCarbonAboveGroundAtInterval + youngPoolSoilCarbonBelowGroundAtInterval + oldPoolSoilCarbonAtInterval + youngPoolManureAtInterval;
        }

        /// <summary>
        /// Equation 2.1.3-16. Year-over-year change in soil organic carbon. Positive when carbon
        /// is accumulating (sequestration), negative when the soil is a net source.
        /// </summary>
        /// <returns>ΔSoilC (kg C ha⁻¹ yr⁻¹).</returns>
        public double CalculateChangeInSoilCarbonAtInterval(
            double soilOrganicCarbonAtInterval,
            double soilOrganicCarbonAtPreviousInterval)
        {
            return soilOrganicCarbonAtInterval - soilOrganicCarbonAtPreviousInterval;
        }

        /// <summary>
        /// Equation 2.1.3-17. Scales the per-hectare ΔSoilC to a per-field absolute value by
        /// multiplying by the field area.
        /// </summary>
        /// <returns>ΔSoilC for the entire field (kg C field⁻¹ yr⁻¹).</returns>
        public double CalculateChangeInSoilOrganicCarbonForFieldAtInterval(
            double changeInSoilOrganicCarbonAtInterval,
            double fieldArea)
        {
            return changeInSoilOrganicCarbonAtInterval * fieldArea;
        }

        /// <summary>
        /// Equation 2.1.3-18. Sums the per-field ΔSoilC values to the farm level. Just a sum —
        /// kept as a named method so the equation reference is greppable.
        /// </summary>
        public double CalculateChangeInSoilOrganicCarbonForFarmAtInterval(
            IEnumerable<double> changeInSoilOrganicCarbonForFields)
        {
            return changeInSoilOrganicCarbonForFields.Sum();
        }

        /// <summary>
        /// Equation 2.1.4-1. Converts soil organic carbon to CO₂-equivalent using the molar-mass
        /// ratio 44/12 (CO₂ : C). Used when surfacing the carbon results in CO₂e units for
        /// GHG-inventory-style reporting.
        /// </summary>
        public double CalculateCarbonDioxideEquivalentsForSoil(double soilOrganicCarbonAtInterval)
        {
            var carbonDioxideEquivalentForSoil = soilOrganicCarbonAtInterval * (44.0 / 12.0);
            return carbonDioxideEquivalentForSoil;
        }

        /// <summary>
        /// Equation 2.1.4-2
        /// </summary>
        public double CalculateChangeInCarbonDioxideEquivalentsForSoil(double changeInSoilOrganicCarbonAtInterval)
        {
            var changeInCarbonDioxideEquivalentsForSoil = changeInSoilOrganicCarbonAtInterval * (44.0 / 12.0);
            return changeInCarbonDioxideEquivalentsForSoil;
        }

        /// <summary>
        /// Equation 2.1.4-3
        /// </summary>
        public double CalculateCarbonDioxideChangeForSoilsByMonth(double changeInCarbonDioxideEquivalentsForSoil)
        {
            var carbonDioxideChangeForSoilByMonth = changeInCarbonDioxideEquivalentsForSoil / 12.0;
            return carbonDioxideChangeForSoilByMonth;
        }

        /// <summary>
        /// Equation 11.3.2-4
        /// </summary>
        /// <returns>Total carbon losses from grazing animals (kg C)</returns>
        public double RecalculatePlantCarbonForGrazingScenario(
            CropViewItem viewItem)
        {
            var lossesFromGrazing = viewItem.TotalCarbonLossesByGrazingAnimals;

            var averageUtilizationRate = viewItem.GrazingViewItems.Any() ? viewItem.GrazingViewItems.Average(x => x.Utilization) : 0;
            var denominator = 1 - (averageUtilizationRate / 100.0);
            if (denominator < 0)
            {
                denominator = 1;
            }

            var uptake = lossesFromGrazing / denominator;

            return uptake;
        }

        public double RecalculateCarbonInputForGrazingScenario(
            CropViewItem viewItem)
        {
            // Check for negative values here

            var result = viewItem.PlantCarbonInAgriculturalProduct - (viewItem.TotalCarbonLossesByGrazingAnimals / viewItem.Area);
            if (result < 0)
            {
                return viewItem.PlantCarbonInAgriculturalProduct;
            }

            return result;
        }

        #endregion
    }
}