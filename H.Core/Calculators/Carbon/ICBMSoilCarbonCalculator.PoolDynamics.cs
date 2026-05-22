using System;
using System.Collections.Generic;
using System.Linq;
using H.Core.Enumerations;
using H.Core.Models;
using H.Core.Models.LandManagement.Fields;

namespace H.Core.Calculators.Carbon
{
    /// <summary>
    /// Partial: the ICBM pool-dynamics orchestration — the equilibrium (year 0) seed and the
    /// per-year carbon interval step. Both drive the primitive pool equations in the main class
    /// file. <c>FieldResultsService.CalculateFinalResultsForField</c> calls
    /// <see cref="CalculateEquilibriumYear"/> once to seed, then <see cref="CalculateCarbonAtInterval"/>
    /// per year (carbon before the per-interval nitrogen in <c>ICBMSoilCarbonCalculator.Nitrogen.cs</c>).
    /// </summary>
    public partial class ICBMSoilCarbonCalculator
    {
        /// <summary>
        /// ICBM per-year step. Reads the four pool values from <paramref name="previousYearResults"/>
        /// and writes the four updated pool values plus <c>SoilCarbon</c> and <c>ChangeInCarbon</c>
        /// onto <paramref name="currentYearResults"/>.
        ///
        /// <para><b>Pool flow per year:</b></para>
        /// <list type="bullet">
        ///   <item><c>YoungPoolSoilCarbonAboveGround</c> ← <see cref="CalculateYoungPoolAboveGroundCarbonAtInterval"/></item>
        ///   <item><c>YoungPoolSoilCarbonBelowGround</c> ← <see cref="CalculateYoungPoolBelowGroundCarbonAtInterval"/></item>
        ///   <item><c>YoungPoolManureCarbon</c> ← <see cref="CalculateYoungPoolManureCarbonAtInterval"/></item>
        ///   <item><c>OldPoolSoilCarbon</c> ← <see cref="CalculateOldPoolSoilCarbonAtInterval"/></item>
        ///   <item><c>SoilCarbon = sum of the four above</c>; <c>ChangeInCarbon = current SoilC − previous SoilC</c></item>
        /// </list>
        ///
        /// <para><b>Climate vs management factor:</b></para>
        /// All four pool calculations take a "climate or management" scalar. Which one is used
        /// depends on <c>farm.Defaults.UseClimateParameterInsteadOfManagementFactor</c>. The
        /// management factor combines climate + tillage; the raw climate parameter ignores
        /// tillage practice. Both calculators are wired up the same way regardless of which is
        /// selected.
        ///
        /// <para><b>Custom starting SOC:</b></para>
        /// When <c>farm.UseCustomStartingSoilOrganicCarbon</c> is true and we're on the first
        /// real year (previousYear == equilibrium year placeholder, Year = 0), we keep the
        /// equilibrium pool values verbatim and override <c>SoilCarbon = StartingSoilOrganicCarbon</c>
        /// so the user-measured value is the visible starting point rather than the calculated
        /// equilibrium.
        /// </para>
        /// </summary>
        public void CalculateCarbonAtInterval(
            CropViewItem previousYearResults,
            CropViewItem currentYearResults,
            Farm farm)
        {
            var climateParameterOrManagementFactor = GetClimateOrManagementFactor(farm, currentYearResults);

            currentYearResults.YoungPoolSoilCarbonAboveGround =
                this.CalculateYoungPoolAboveGroundCarbonAtInterval(
                    youngPoolAboveGroundCarbonAtPreviousInterval: previousYearResults.YoungPoolSoilCarbonAboveGround,
                    aboveGroundCarbonAtPreviousInterval: previousYearResults.CombinedAboveGroundInput,
                    youngPoolDecompositionRate: farm.Defaults.DecompositionRateConstantYoungPool,
                    climateParameter: climateParameterOrManagementFactor);

            currentYearResults.YoungPoolSoilCarbonBelowGround =
                this.CalculateYoungPoolBelowGroundCarbonAtInterval(
                    youngPoolBelowGroundCarbonAtPreviousInterval: previousYearResults.YoungPoolSoilCarbonBelowGround,
                    belowGroundCarbonAtPreviousInterval: previousYearResults.CombinedBelowGroundInput,
                    youngPoolDecompositionRate: farm.Defaults.DecompositionRateConstantYoungPool,
                    climateParameter: climateParameterOrManagementFactor);

            currentYearResults.YoungPoolManureCarbon =
                this.CalculateYoungPoolManureCarbonAtInterval(
                    youngPoolManureCarbonAtPreviousInterval: previousYearResults.YoungPoolManureCarbon,
                    manureCarbonInputAtPreviousInterval: previousYearResults.CombinedManureAndDigestateInput,
                    youngPoolDecompositionRate: farm.Defaults.DecompositionRateConstantYoungPool,
                    climateParameter: climateParameterOrManagementFactor);

            currentYearResults.OldPoolSoilCarbon = this.CalculateOldPoolSoilCarbonAtInterval(
                oldPoolSoilCarbonAtPreviousInterval: previousYearResults.OldPoolSoilCarbon,
                aboveGroundHumificationCoefficient: farm.Defaults.HumificationCoefficientAboveGround,
                belowGroundHumificationCoefficient: farm.Defaults.HumificationCoefficientBelowGround,
                youngPoolDecompositionRate: farm.Defaults.DecompositionRateConstantYoungPool,
                oldPoolDecompositionRate: farm.Defaults.DecompositionRateConstantOldPool,
                youngPoolAboveGroundOrganicCarbonAtPreviousInterval: previousYearResults.YoungPoolSoilCarbonAboveGround,
                youngPoolBelowGroundOrganicCarbonAtPreviousInterval: previousYearResults.YoungPoolSoilCarbonBelowGround,
                aboveGroundCarbonResidueAtPreviousInterval: previousYearResults.CombinedAboveGroundInput,
                belowGroundCarbonResidueAtPreviousInterval: previousYearResults.CombinedBelowGroundInput,
                climateParameter: climateParameterOrManagementFactor,
                youngPoolManureAtPreviousInterval: previousYearResults.YoungPoolManureCarbon,
                manureHumificationCoefficient: farm.Defaults.HumificationCoefficientManure,
                manureCarbonInputAtPreviousInterval: previousYearResults.CombinedManureAndDigestateInput);

            currentYearResults.SoilCarbon = this.CalculateSoilCarbonAtInterval(
                youngPoolSoilCarbonAboveGroundAtInterval: currentYearResults.YoungPoolSoilCarbonAboveGround,
                youngPoolSoilCarbonBelowGroundAtInterval: currentYearResults.YoungPoolSoilCarbonBelowGround,
                oldPoolSoilCarbonAtInterval: currentYearResults.OldPoolSoilCarbon,
                youngPoolManureAtInterval: currentYearResults.YoungPoolManureCarbon);

            currentYearResults.ChangeInCarbon = this.CalculateChangeInSoilCarbonAtInterval(
                soilOrganicCarbonAtInterval: currentYearResults.SoilCarbon,
                soilOrganicCarbonAtPreviousInterval: previousYearResults.SoilCarbon);

            // If there is a measured value, then use the calculated Y_ag, Y_bg, and O pool values for the current year (using 2.1.3-8, 2.1.3-9, and 2.1.3-10)
            if (previousYearResults.Year == 0 && farm.UseCustomStartingSoilOrganicCarbon)
            {
                currentYearResults.YoungPoolSoilCarbonAboveGround = previousYearResults.YoungPoolSoilCarbonAboveGround;
                currentYearResults.YoungPoolSoilCarbonBelowGround = previousYearResults.YoungPoolSoilCarbonBelowGround;
                currentYearResults.YoungPoolManureCarbon = previousYearResults.YoungPoolManureCarbon;
                currentYearResults.OldPoolSoilCarbon = previousYearResults.OldPoolSoilCarbon;
                currentYearResults.SoilCarbon = farm.StartingSoilOrganicCarbon;
                currentYearResults.ChangeInCarbon = 0;
            }
        }

        /// <summary>
        /// Builds the synthetic "year 0" <see cref="CropViewItem"/> that ICBM uses to seed its
        /// per-year pool dynamics. Averages the rotation's C inputs + climate / management
        /// factors and then runs the steady-state ICBM equations (see
        /// <see cref="CalculateYoungPoolSteadyStateAboveGround"/> etc.) to compute what each pool
        /// would hold if the input / decay rates were sustained indefinitely.
        ///
        /// <para>
        /// The returned item is not added to the farm's stage state — it's used only as
        /// <c>previousYearResults</c> for the very first iteration of the per-year loop in
        /// <c>FieldResultsService.CalculateFinalResultsForField</c>.
        /// </para>
        ///
        /// <para><b>Rotation-size handling:</b></para>
        /// Reads <c>SizeOfFirstRotationForField</c> from the first detail view item. On legacy
        /// (v4-imported) farms this can be zero; we fall back to
        /// <c>FieldSystemComponent.SizeOfFirstRotationInField()</c> to recover a sensible value
        /// rather than averaging over zero items.
        ///
        /// <para><b>Custom starting SOC branch:</b></para>
        /// When <c>farm.UseCustomStartingSoilOrganicCarbon</c> is true, the young pools are
        /// derived from the user's measured value (equations 2.1.3-8, 2.1.3-9) and the old pool
        /// takes whatever's left over (equation 2.1.3-10). When false, all four pools come from
        /// the analytic steady-state expressions.
        /// </summary>
        public CropViewItem CalculateEquilibriumYear(
            List<CropViewItem> detailViewItems,
            Farm farm,
            Guid componentId)
        {
            var fieldSystemComponent = farm.GetFieldSystemComponent(componentId);

            // Get the field system component that will be used to calculate the equilibrium year
            var sizeOfFirstRotationForField =
                detailViewItems.OrderBy(viewItem => viewItem.Year).First().SizeOfFirstRotationForField;
            if (sizeOfFirstRotationForField == 0 && fieldSystemComponent != null)
            {
                // Was not set during creation of old farm
                sizeOfFirstRotationForField = fieldSystemComponent.SizeOfFirstRotationInField();
            }

            // Get the detail view items that represent the crops that define the first rotation
            var viewItemsInRotation = detailViewItems.Take(sizeOfFirstRotationForField).ToList();

            /*
             * Calculate averages for the crops that define the first rotation - this is used to calculate the equilibrium state
             *
             * Use a specified strategy to calculate these starting values
             */

            var equilibriumAboveGroundInput = 0d;
            var equilibriumBelowGroundInput = 0d;
            var equilibriumManureInput = 0d;
            var equilibriumClimateParameter = 0d;
            var equilibriumManagementFactor = 0d;
            var equilibriumDigestateInput = 0d;

            var equilibriumAboveGroundNitrogen = 0d;
            var equilibriumBelowGroundNitrogen = 0d;

            var strategy = farm.Defaults.EquilibriumCalculationStrategy;
            if (strategy == EquilibriumCalculationStrategies.CarMultipleYearAverage)
            {
                // Not implemented yet
            }
            else
            {
                // At this point, the detail view items have had their C inputs calculated
                // Equation 2.1.3-1
                equilibriumAboveGroundInput = viewItemsInRotation.Average(x => x.CombinedAboveGroundInput);

                // Equation 2.1.3-2
                equilibriumBelowGroundInput = viewItemsInRotation.Average(x => x.CombinedBelowGroundInput);

                // Equation 2.1.3-3
                equilibriumManureInput = viewItemsInRotation.Average(x => x.CombinedManureInput);
                equilibriumDigestateInput = viewItemsInRotation.Average(x => x.CombinedDigestateInput);

                equilibriumClimateParameter = viewItemsInRotation.Average(x => x.ClimateParameter);
                equilibriumManagementFactor = viewItemsInRotation.Average(x => x.ManagementFactor);

                equilibriumAboveGroundNitrogen = viewItemsInRotation.Average(x => x.CombinedAboveGroundResidueNitrogen);
                equilibriumBelowGroundNitrogen = viewItemsInRotation.Average(x => x.CombinedBelowGroundResidueNitrogen);
            }

            // This is the equilibrium year result
            var result = new CropViewItem();

            result.CombinedAboveGroundInput = equilibriumAboveGroundInput;
            result.CombinedBelowGroundInput = equilibriumBelowGroundInput;

            result.CombinedAboveGroundResidueNitrogen = equilibriumAboveGroundNitrogen;
            result.CombinedBelowGroundResidueNitrogen = equilibriumBelowGroundNitrogen;

            result.CombinedManureInput = equilibriumManureInput;
            result.CombinedDigestateInput = equilibriumDigestateInput;
            result.ClimateParameter = equilibriumClimateParameter;
            result.ManagementFactor = equilibriumManagementFactor;

            var averageNitrogenConcentrationInProduct = viewItemsInRotation.Select(x => x.NitrogenContentInProduct).Average();
            var averageNitrogenConcentrationInStraw = viewItemsInRotation.Select(x => x.NitrogenContentInStraw).Average();
            var averageNitrogenConcentrationInRoots = viewItemsInRotation.Select(x => x.NitrogenContentInRoots).Average();
            var averageNitrogenConcentrationInExtraroots = viewItemsInRotation.Select(x => x.NitrogenContentInExtraroot).Average();

            result.NitrogenContentInProduct = averageNitrogenConcentrationInProduct;
            result.NitrogenContentInStraw = averageNitrogenConcentrationInStraw;
            result.NitrogenContentInRoots = averageNitrogenConcentrationInRoots;
            result.NitrogenContentInExtraroot = averageNitrogenConcentrationInExtraroots;

            /*
             * Carbon
             */

            var climateOrManagementFactor = GetClimateOrManagementFactor(farm, result);

            if (farm.UseCustomStartingSoilOrganicCarbon == false)
            {
                result.YoungPoolSoilCarbonAboveGround =
                    this.CalculateYoungPoolSteadyStateAboveGround(
                        averageAboveGroundCarbonInput: result.CombinedAboveGroundInput,
                        youngPoolDecompositionRate: farm.Defaults.DecompositionRateConstantYoungPool,
                        climateParameter: climateOrManagementFactor);
            }
            else
            {

                // Equation 2.1.3-8
                var youngPoolAboveGround = result.CombinedAboveGroundInput /
                                           (climateOrManagementFactor *
                                            farm.Defaults.DecompositionRateConstantYoungPool);

                result.YoungPoolSoilCarbonAboveGround = youngPoolAboveGround;
            }

            if (farm.UseCustomStartingSoilOrganicCarbon == false)
            {
                result.YoungPoolSoilCarbonBelowGround =
                    this.CalculateYoungPoolSteadyStateBelowGround(
                        averageBelowGroundCarbonInput: result.CombinedBelowGroundInput,
                        youngPoolDecompositionRate: farm.Defaults.DecompositionRateConstantYoungPool,
                        climateParameter: climateOrManagementFactor);
            }
            else
            {
                // Equation 2.1.3-9
                var youngPoolBelowGround = result.CombinedBelowGroundInput /
                                           (climateOrManagementFactor *
                                            farm.Defaults.DecompositionRateConstantYoungPool);

                result.YoungPoolSoilCarbonBelowGround = youngPoolBelowGround;
            }

            result.YoungPoolManureCarbon = this.CalculateYoungPoolSteadyStateManure(
                averageManureCarbonInput: (result.CombinedManureInput + result.CombinedDigestateInput),
                youngPoolDecompositionRate: farm.Defaults.DecompositionRateConstantYoungPool,
                climateParameter: climateOrManagementFactor);

            if (farm.UseCustomStartingSoilOrganicCarbon == false)
            {
                result.OldPoolSoilCarbon = this.CalculateOldPoolSteadyState(
                    youngPoolDecompositionRate: farm.Defaults.DecompositionRateConstantYoungPool,
                    oldPoolDecompositionRate: farm.Defaults.DecompositionRateConstantOldPool,
                    climateParameter: climateOrManagementFactor,
                    aboveGroundHumificationCoefficient: farm.Defaults.HumificationCoefficientAboveGround,
                    belowGroundHumificationCoefficient: farm.Defaults.HumificationCoefficientBelowGround,
                    averageAboveGroundCarbonInputOfRotation: result.CombinedAboveGroundInput,
                    averageBelowGroundCarbonInputOfRotation: result.CombinedBelowGroundInput,
                    aboveGroundYoungPoolSteadyState: result.YoungPoolSoilCarbonAboveGround,
                    belowGroundYoungPoolSteadyState: result.YoungPoolSoilCarbonBelowGround,
                    manureYoungPoolSteadyState: result.YoungPoolSteadyStateManure,
                    averageManureCarbonInputOfRotation: (result.CombinedManureInput + result.CombinedDigestateInput),
                    manureHumificationCoefficient: farm.Defaults.HumificationCoefficientManure);
            }
            else
            {
                // Equation 2.1.3-10
                result.OldPoolSoilCarbon = farm.StartingSoilOrganicCarbon -
                                           (result.YoungPoolSoilCarbonAboveGround +
                                            result.YoungPoolSoilCarbonBelowGround);
            }

            result.SoilCarbon = this.CalculateSoilCarbonAtInterval(
                youngPoolSoilCarbonAboveGroundAtInterval: result.YoungPoolSoilCarbonAboveGround,
                youngPoolSoilCarbonBelowGroundAtInterval: result.YoungPoolSoilCarbonBelowGround,
                oldPoolSoilCarbonAtInterval: result.OldPoolSoilCarbon,
                youngPoolManureAtInterval: result.YoungPoolManureCarbon);

            return result;
        }
    }
}
