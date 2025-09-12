using H.Core.Enumerations;
using H.Core.Providers.Feed;

namespace H.Core.Services.DietService
{
    public interface IDietFactory
    {
        IDiet Create();
        IDiet Create(DietType dietType, AnimalType animalType);
        IReadOnlyList<Tuple<AnimalType, DietType>> GetValidDietKeys();
        bool IsValidDietType(AnimalType animalType, DietType dietType);
    }
}