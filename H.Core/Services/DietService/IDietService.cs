using H.Core.Enumerations;
using H.Core.Providers.Feed;

namespace H.Core.Services.DietService;

public interface IDietService
{
    IReadOnlyList<AnimalType> GetValidAnimalDietTypes(AnimalType animalType);
    IReadOnlyList<IDiet> GetDiets();
    IDiet GetNoDiet();
    IDiet GetDiet(AnimalType animalType, DietType dietType);
}