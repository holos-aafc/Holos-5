using System.Globalization;
using H.Core.Emissions.Results;
using H.Core.Enumerations;
using H.Core.Models;
using H.Core.Models.LandManagement.Fields;
using H.Core.Providers.Carbon;

namespace H.Core.Services.LandManagement
{
    public interface IFieldResultsService
    {
        double CalculateClimateParameter(CropViewItem viewItem, Farm farm);
        double CalculateTillageFactor(CropViewItem viewItem, Farm farm);
        double CalculateManagementFactor(double climateParameter, double tillageFactor);
        double CalculateRequiredNitrogenFertilizer(Farm farm, CropViewItem viewItem,
                                                   FertilizerApplicationViewItem fertilizerApplicationViewItem);
        void CreateDetailViewItems(Farm farm);


        List<CropViewItem> CalculateFinalResults(IEnumerable<Farm> farms);

        bool ExportResultsToFile(IEnumerable<CropViewItem> results,
                                            object path,
                                            CultureInfo cultureInfo,
                                            MeasurementSystemType measurementSystemType,
                                            string languageAddOn,
                                            bool exportedFromGui,
                                            Farm farm);
        double CalculateHarvest(CropViewItem viewItem);

        FieldSystemDetailsStageState GetStageState(Farm farm);
        void AssignDefaultNitrogenFertilizerRate(CropViewItem viewItem, Farm farm,
                                                 FertilizerApplicationViewItem fertilizerApplicationViewItem);
        Table_7_Relative_Biomass_Information_Data? GetResidueData(CropViewItem cropViewItem, Farm farm);
        CropViewItem MapDetailsScreenViewItemFromComponentScreenViewItem(CropViewItem viewItem, int year);
        void InitializeStageState(Farm farm);
        List<CropViewItem> CalculateFinalResults(Farm farm);

        /// <summary>
        /// Equation 2.5.2-6
        /// </summary>
        /// <returns>The amount of product required (kg product ha^-1)</returns>
        double CalculateAmountOfProductRequired(
            Farm farm,
            CropViewItem viewItem,
            FertilizerApplicationViewItem fertilizerApplicationViewItem);

        List<AnimalComponentEmissionsResults> AnimalResults { get; set; }
    }
}
