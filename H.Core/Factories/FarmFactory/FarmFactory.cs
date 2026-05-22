using DynamicData;
using H.Core.Services.DietService;
using H.Infrastructure.Services;
using Microsoft.Extensions.Logging;

namespace H.Core.Factories.FarmFactory;

public class FarmFactory : IFarmFactory
{
    #region Fields

    private readonly IDietService _dietService;
    private ICacheService _cacheService;
    private ILogger _logger;

    #endregion

    #region Constructors

    public FarmFactory(IDietService dietService, ICacheService cacheService, ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));

        _dietService = dietService ?? throw new ArgumentNullException(nameof(dietService));
    }

    #endregion

    #region Public Methods
    
    public IFarmDto Create()
    {
        var farm = new FarmDto();

        var diets = _dietService.GetDiets();
        farm.Diets.AddRange(diets);

        return farm;
    } 

    #endregion
}