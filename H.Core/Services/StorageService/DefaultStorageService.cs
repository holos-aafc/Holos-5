using H.Core.Models;

namespace H.Core.Services.StorageService;

public class DefaultStorageService : IStorageService
{
    #region Fields

    private readonly IStorage _storage;

    #endregion

    #region Constructors

    public DefaultStorageService(IStorage storage)
    {
        _storage = storage;
    }

    #region Public Methods

    #endregion

    #endregion

    #region Public Methods

    public Farm GetActiveFarm()
    {
        return _storage.ApplicationData.GlobalSettings.ActiveFarm;
    }

    public List<Farm> GetAllFarms()
    {
        return _storage.ApplicationData.Farms.ToList();
    }

    public bool SetActiveFarm(Farm farm)
    {
        if (farm != null)
        {
            _storage.ApplicationData.GlobalSettings.ActiveFarm = farm;

            return true;
        }

        return false;
    }

    #endregion
}