using H.Core.Enumerations;

namespace H.Core.Services.DietService;

public interface IDietService
{
    List<AnimalType> GetValidAnimalDietTypes(AnimalType animalType);
}