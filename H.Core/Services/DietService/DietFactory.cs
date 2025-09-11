using H.Core.Enumerations;
using H.Core.Providers.Feed;
using Microsoft.Extensions.Logging;

namespace H.Core.Services.DietService
{
    public class DietFactory : IDietFactory
    {
        #region Fields

        private readonly ILogger _logger;

        #endregion

        #region Constructors

        public DietFactory(ILogger logger)
        {
            if (logger != null)
            {
                _logger = logger; 
            }
            else
            {
                throw new ArgumentNullException(nameof(logger));
            }
        }

        #endregion

        #region Public Methods

        public IDiet Create()
        {
            throw new NotImplementedException();
        }

        public IDiet Create(DietType dietType, AnimalType animalType)
        {
            if (this.IsValidDietType(animalType, dietType))
            {
                _logger.LogInformation($"Creating diet for {dietType} and {animalType}");

                return new Diet() { Name = "Holos Diet" };
            }
            else
            {
                _logger.LogError($"Cannot create {dietType} for {animalType}");

                return new Diet()
                {
                    Name = "Unknown diet"
                };
            }
        }

        public IReadOnlyList<Tuple<AnimalType, DietType>> GetValidDiets()
        {
            var dietList = new List<Tuple<AnimalType, DietType>>()
            {
                new (AnimalType.BeefCow, DietType.LowEnergyAndProtein),
                new (AnimalType.BeefCow, DietType.MediumEnergyAndProtein)
            };

            return dietList;
        }

        public bool IsValidDietType(AnimalType animalType, DietType dietType)
        {
            return this.GetValidDiets().Contains(new Tuple<AnimalType, DietType>(animalType, dietType));
        }

        #endregion
    }
}