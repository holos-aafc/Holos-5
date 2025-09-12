using System.Collections.Generic;
using H.Core.Enumerations;

namespace H.Core.Providers.Feed
{
    public interface IFeedIngredientProvider
    {
        IList<FeedIngredient> GetBeefFeedIngredients();
        IList<FeedIngredient> GetDairyFeedIngredients();
        IList<FeedIngredient> GetSwineFeedIngredients();
        IReadOnlyCollection<IFeedIngredient> GetIngredientsForDiet(AnimalType animalType, DietType dietType);
        
        FeedIngredient CopyIngredient(FeedIngredient ingredient, double defaultPercentageInDiet);
    }
}