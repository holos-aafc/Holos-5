using System.Collections.Generic;
using H.Core.Models;
using H.Core.Models.LandManagement.Fields;

namespace H.Core.Services.LandManagement
{
    /// <summary>
    /// Partial: command-line-only residue/nitrogen input fill. The GUI assigns these inputs
    /// interactively during stage-state build, but farms loaded from the CLI input files may leave
    /// prerequisites at zero as an opt-in signal for Holos to compute them. <see cref="ProcessCommandLineItems"/>
    /// runs at the top of <c>CalculateFinalResultsForField</c> and is a no-op in GUI mode.
    /// </summary>
    public partial class FieldResultsService
    {
        /// <summary>
        /// Command-line residue-input fill. For farms loaded from the CLI input files, recalculate
        /// any carbon- and nitrogen-input prerequisites the user left at zero — their opt-in signal
        /// to have Holos compute them. A no-op in GUI mode, where these are already assigned during
        /// stage-state build. Every step is guarded on a zero value, so user-supplied non-zero
        /// values are preserved.
        ///
        /// <para>
        /// The carbon model cannot run without above- and below-ground residue inputs, so when the
        /// user has zeroed both, default biomass coefficients are assigned and the inputs are
        /// recomputed via the ICBM input calculator (which also re-derives manure inputs). The
        /// nitrogen pipeline reads the per-residue nitrogen contents and the fixation percentage,
        /// so those are filled in the same pass.
        /// </para>
        /// </summary>
        public void ProcessCommandLineItems(List<CropViewItem> viewItems, Farm farm)
        {
            if (farm.IsCommandLineMode == false)
            {
                return;
            }

            foreach (var item in viewItems)
            {
                var adjoiningYears = this.GetAdjoiningYears(viewItems, item.Year);
                var currentYearViewItem = adjoiningYears.CurrentYearViewItem;
                if (currentYearViewItem == null)
                {
                    continue;
                }

                if (currentYearViewItem.Yield == 0)
                {
                    _cropInitializationService.InitializeYield(currentYearViewItem, farm);
                }

                if (currentYearViewItem.LigninContent == 0)
                {
                    _cropInitializationService.InitializeLigninContent(currentYearViewItem, farm);
                }

                if (currentYearViewItem.MoistureContentOfCropPercentage == 0)
                {
                    _cropInitializationService.InitializeMoistureContent(currentYearViewItem, farm);
                }

                var cropResidueInputsNeedRecalculating =
                    currentYearViewItem.AboveGroundCarbonInput == 0 &&
                    currentYearViewItem.BelowGroundCarbonInput == 0;
                if (cropResidueInputsNeedRecalculating)
                {
                    _cropInitializationService.InitializeBiomassCoefficients(currentYearViewItem, farm);

                    _icbmCarbonInputCalculator.AssignInputs(
                        previousYearViewItem: adjoiningYears.PreviousYearViewItem!,
                        currentYearViewItem: currentYearViewItem,
                        nextYearViewItem: adjoiningYears.NextYearViewItem!,
                        farm: farm,
                        animalResults: this.AnimalResults);
                }

                // Nitrogen prerequisites: the residue-N pipeline reads the per-residue nitrogen
                // contents and the fixation percentage. If the user left the nitrogen contents at
                // zero, fill the defaults; the fixation percentage is (re)derived from the crop type
                // (zero for non-legumes, which is correct).
                var nitrogenContentNeedsRecalculating =
                    currentYearViewItem.NitrogenContentInProduct == 0 &&
                    currentYearViewItem.NitrogenContentInStraw == 0 &&
                    currentYearViewItem.NitrogenContentInRoots == 0 &&
                    currentYearViewItem.NitrogenContentInExtraroot == 0;
                if (nitrogenContentNeedsRecalculating)
                {
                    _cropInitializationService.InitializeNitrogenContent(currentYearViewItem, farm);
                }

                _cropInitializationService.InitializeNitrogenFixation(currentYearViewItem);

                // Command-line farms have no interactive step to build grazing view items; generate
                // them from the farm's animal management periods so grazing-animal carbon and
                // nitrogen contributions (manure on pasture, supplemental feed) are included.
                var field = farm.GetFieldSystemComponent(currentYearViewItem.FieldSystemComponentGuid);
                if (field != null)
                {
                    _cropInitializationService.InitializeGrazingViewItems(farm, currentYearViewItem, field);
                }
            }
        }
    }
}
