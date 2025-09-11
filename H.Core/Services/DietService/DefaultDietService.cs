using H.Core.Enumerations;

namespace H.Core.Services.DietService;

public class DefaultDietService : IDietService
{
    #region Public Methods
    
    public List<AnimalType> GetValidAnimalDietTypes(AnimalType animalType)
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

    #endregion
}