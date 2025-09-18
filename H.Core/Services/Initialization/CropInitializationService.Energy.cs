using H.Core.Factories;
using H.Core.Models;
using H.Core.Models.LandManagement.Fields;

namespace H.Core.Services.Initialization;

public partial class CropInitializationService
{
    #region Public Methods

    public virtual void InitializeFuelEnergy(Farm farm)
    {
        var viewItems = farm.GetAllCropViewItems();
        foreach (var viewItem in viewItems)
        {
            InitializeFuelEnergy(farm, viewItem);
        }
    }

    public virtual void InitializeFuelEnergy(Farm farm, CropViewItem viewItem)
    {
        var soilData = farm.GetPreferredSoilData(viewItem);
        var fuelEnergyEstimates = _table50FuelEnergyEstimatesProvider.GetFuelEnergyEstimatesDataInstance(
            province: soilData.Province,
            soilCategory: soilData.SoilFunctionalCategory,
            tillageType: viewItem.TillageType,
            cropType: viewItem.CropType);

        if (fuelEnergyEstimates != null)
        {
            viewItem.FuelEnergy = fuelEnergyEstimates.FuelEstimate;
        }
    }

    #endregion
}