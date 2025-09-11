using System.ComponentModel.Design;
using H.Core.Enumerations;
using H.Core.Providers.Feed;
using Microsoft.Extensions.Logging;

namespace H.Core.Services.DietService;

public class DefaultDietService : IDietService
{
    #region Fields

    private IDietProvider _dietProvider;
    private IFeedIngredientProvider _feedIngredientProvider;
    private ILogger _logger;
    private IDietFactory _dietFactory;

    #endregion

    #region Constructors 

    public DefaultDietService(IDietProvider dietProvider, IFeedIngredientProvider feedIngredientProvider, ILogger logger, IDietFactory dietFactory)
    {
        if (dietFactory != null)
        {
            _dietFactory = dietFactory;
        }
        else
        {
            throw new ArgumentNullException(nameof(dietFactory));
        }

        if (logger != null)
        {
            _logger = logger; 
        }
        else
        {
            throw new ArgumentNullException(nameof(logger));
        }

        if (dietProvider != null)
        {
            _dietProvider = dietProvider;
        }
        else
        {
            throw new ArgumentNullException(nameof(dietProvider));
        }

        if (feedIngredientProvider != null)
        {
            _feedIngredientProvider = feedIngredientProvider;
        }
        else
        {
            throw new ArgumentNullException(nameof(feedIngredientProvider));
        }
    } 

    #endregion

    #region Public Methods

    public IReadOnlyList<AnimalType> GetValidAnimalDietTypes(AnimalType animalType)
    {
        if (animalType.IsBeefCattleType())
        {
            return new List<AnimalType>()
            {
                AnimalType.BeefBackgrounder,
                AnimalType.BeefFinisher,
                AnimalType.BeefCow,
                AnimalType.BeefBulls,
                AnimalType.Stockers,
            };
        }

        if (animalType.IsDairyCattleType())
        {
            return new List<AnimalType>()
            {
                AnimalType.DairyDryCow,
                AnimalType.DairyHeifers,
                AnimalType.DairyLactatingCow,
            };
        }

        // Sheep default diets don't specify which diets belong to which animal groups. Use these diets for all sheep groups
        if (animalType.IsSheepType())
        {
            return new List<AnimalType>()
            {
                AnimalType.Sheep,
            };
        }

        if (animalType.IsSwineType())
        {
            return new List<AnimalType>()
            {
                AnimalType.Swine,
                AnimalType.SwineBoar,
                AnimalType.SwineDrySow,
                AnimalType.SwineFinisher,
                AnimalType.SwineGrower,
                AnimalType.SwineLactatingSow,
                AnimalType.SwineStarter,
            };
        }

        return new List<AnimalType>();
    }

    public IReadOnlyList<IDiet> GetDiets()
    {
        var validDietTypes = _dietFactory.GetValidDiets();
        var result = new List<IDiet>();

        foreach (var validDietType in validDietTypes)
        {
            var animalType = validDietType.Item1;
            var dietType = validDietType.Item2;

            var diet = _dietFactory.Create(dietType, animalType);

            result.Add(diet);
        }

        return result;
    }

    /// <summary>
    /// Some animal groups will not have a diet (poultry, other livestock, suckling pigs, etc.). In these cases, a non-null diet must still be set.
    /// </summary>
    public IDiet GetNoDiet()
    {
        var diet = _dietProvider.GetNoDiet();

        throw new NotImplementedException();
    }

    #endregion
}