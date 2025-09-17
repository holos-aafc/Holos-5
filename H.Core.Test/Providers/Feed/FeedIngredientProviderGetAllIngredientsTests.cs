using Xunit;
using H.Core.Enumerations;
using H.Core.Providers.Feed;
using System.Linq;

namespace H.Core.Test.Providers.Feed
{
    /// <summary>
    /// Unit tests for the new GetAllIngredientsForAnimalType method
    /// </summary>
    public class FeedIngredientProviderTests
    {
        [Fact]
        public void GetAllIngredientsForAnimalType_BeefAnimalType_ReturnsIngredients()
        {
            // Arrange
            var provider = new FeedIngredientProvider();
            
            // Act
            var result = provider.GetAllIngredientsForAnimalType(AnimalType.Beef);
            
            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.All(result, ingredient => Assert.NotNull(ingredient));
        }

        [Fact]
        public void GetAllIngredientsForAnimalType_DairyAnimalType_ReturnsIngredients()
        {
            // Arrange
            var provider = new FeedIngredientProvider();
            
            // Act
            var result = provider.GetAllIngredientsForAnimalType(AnimalType.Dairy);
            
            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.All(result, ingredient => Assert.NotNull(ingredient));
        }

        [Fact]
        public void GetAllIngredientsForAnimalType_SwineAnimalType_ReturnsIngredients()
        {
            // Arrange
            var provider = new FeedIngredientProvider();
            
            // Act
            var result = provider.GetAllIngredientsForAnimalType(AnimalType.Swine);
            
            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.All(result, ingredient => Assert.NotNull(ingredient));
        }

        [Fact]
        public void GetAllIngredientsForAnimalType_SheepAnimalType_ReturnsEmptyCollection()
        {
            // Arrange
            var provider = new FeedIngredientProvider();
            
            // Act
            var result = provider.GetAllIngredientsForAnimalType(AnimalType.Sheep);
            
            // Assert
            Assert.NotNull(result);
            Assert.Empty(result); // Sheep doesn't have feed ingredient data yet
        }

        [Fact]
        public void GetAllIngredientsForAnimalType_ValidateIngredientProperties()
        {
            // Arrange
            var provider = new FeedIngredientProvider();
            
            // Act
            var beefIngredients = provider.GetAllIngredientsForAnimalType(AnimalType.Beef);
            
            // Assert
            Assert.NotEmpty(beefIngredients);
            
            var firstIngredient = beefIngredients.First();
            Assert.NotEqual(IngredientType.NotSelected, firstIngredient.IngredientType);
            Assert.NotNull(firstIngredient.IngredientTypeString);
            Assert.True(firstIngredient.DryMatter >= 0);
            Assert.True(firstIngredient.CrudeProtein >= 0);
        }

        [Theory]
        [InlineData(AnimalType.Beef)]
        [InlineData(AnimalType.Dairy)]
        [InlineData(AnimalType.Swine)]
        public void GetAllIngredientsForAnimalType_ValidAnimalTypes_ReturnsSameAsSpecificMethods(AnimalType animalType)
        {
            // Arrange
            var provider = new FeedIngredientProvider();
            
            // Act
            var newMethodResult = provider.GetAllIngredientsForAnimalType(animalType);
            var existingMethodResult = animalType switch
            {
                AnimalType.Beef => provider.GetBeefFeedIngredients(),
                AnimalType.Dairy => provider.GetDairyFeedIngredients(),
                AnimalType.Swine => provider.GetSwineFeedIngredients(),
                _ => null
            };
            
            // Assert
            Assert.NotNull(existingMethodResult);
            Assert.Equal(existingMethodResult.Count, newMethodResult.Count);
            
            // Verify all ingredients match (by IngredientType)
            var newMethodTypes = newMethodResult.Select(i => i.IngredientType).OrderBy(t => t);
            var existingMethodTypes = existingMethodResult.Select(i => i.IngredientType).OrderBy(t => t);
            Assert.Equal(existingMethodTypes, newMethodTypes);
        }
    }
}