using H.Core.Enumerations;

namespace H.Core.Providers.Feed;

public class DietDto : IDietDto
{
    public bool IsDefaultDiet { get; set; }
    public string Name { get; set; }
    public DietType DietType { get; set; }
    public AnimalType AnimalType { get; set; }
    public double MethaneConversionFactor { get; set; }
    public double DietaryNetEnergyConcentration { get; set; }
    public IReadOnlyCollection<IFeedIngredient> Ingredients { get; set; }
}