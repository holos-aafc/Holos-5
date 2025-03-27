using H.Core.Models;

namespace H.Core.Services.StorageService;

public class DefaultStorageService : IStorageService
{
    
    #region Fields

    #endregion

    #region Constructors

    public DefaultStorageService(IStorage storage)
    {
        this.Storage = storage;
    }

    #endregion

    #region Properties

    public IStorage Storage { get;  set; }

    #endregion

    #region Public Methods

    public Farm GetActiveFarm()
    {
        return Storage.ApplicationData.GlobalSettings.ActiveFarm;
    }

    public List<Farm> GetAllFarms()
    {
        return Storage.ApplicationData.Farms.ToList();
    }

    public bool SetActiveFarm(Farm farm)
    {
        if (farm != null)
        {
            Storage.ApplicationData.GlobalSettings.ActiveFarm = farm;

            return true;
        }

        return false;
    }

    public void AddFarm(Farm farm)
    {
        if (farm != null)
        {
            Storage.ApplicationData.Farms.Add(farm);
        }
    }

    #endregion
}