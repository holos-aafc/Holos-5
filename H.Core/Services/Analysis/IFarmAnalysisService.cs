using H.Core.Models;
using H.Core.Models.Results;

namespace H.Core.Services.Analysis;

/// <summary>
/// Façade over the carbon/nitrogen calculator stack for GUI consumers. Wraps
/// <c>FieldResultsService.CalculateFinalResults</c> + the animal emissions pipeline and produces a
/// flat <see cref="FarmAnalysisResults"/> DTO that view models can bind to without depending on
/// CropViewItem internals.
///
/// Currently a single synchronous method that runs the full analysis pipeline; cancellation,
/// progress, and partial-result reporting may be added later.
/// </summary>
public interface IFarmAnalysisService
{
    /// <summary>
    /// Runs the full carbon/nitrogen analysis for a farm and returns flattened per-year results.
    /// Returns <see cref="FarmAnalysisResults"/> with <c>IsEmpty == true</c> when the farm has no
    /// fields or no detail view items.
    /// </summary>
    FarmAnalysisResults RunAnalysis(Farm farm);
}
