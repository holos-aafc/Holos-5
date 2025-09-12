using H.Core.Enumerations;
using H.Core.Providers.Feed;
using H.Infrastructure.Services;
using Microsoft.Extensions.Logging;

namespace H.Core.Services.DietService
{
    public class DietFactory : IDietFactory
    {
        #region Fields

        private readonly ILogger _logger;
        private readonly ICacheService _cacheService;
        private readonly IReadOnlyList<Tuple<AnimalType, DietType>> _validDietKeys;

        #endregion

        #region Constructors

        public DietFactory()
        {
            _validDietKeys = this.CreateDietKeys();
        }

        public DietFactory(ILogger logger, ICacheService cacheService) : this()
        {
            if (cacheService != null)
            {
                _cacheService = cacheService;
            }
            else
            {
                throw new ArgumentNullException(nameof(cacheService));
            }

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
                var key = $"{nameof(DietFactory.Create)}_{dietType}_{animalType}";
                var cachedDiet = _cacheService.Get<IDiet>(key);
                if (cachedDiet != null)
                {
                    _logger.LogInformation($"Returning cached diet for {dietType} and {animalType}");
                    return cachedDiet;
                }

                _logger.LogInformation($"Creating diet for {dietType} and {animalType}");
                var diet = new Diet() { Name = "Holos Diet" };

                // Add the newly created diet to the cache before returning
                _cacheService.Set(key, diet);

                return diet;
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

        public IReadOnlyList<Tuple<AnimalType, DietType>> GetValidDietKeys()
        {
            return _validDietKeys;
        }

        public bool IsValidDietType(AnimalType animalType, DietType dietType)
        {
            return this.GetValidDietKeys().Contains(new Tuple<AnimalType, DietType>(animalType, dietType));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// These are the combinations of diet types and animal types for which we have data.
        /// </summary>
        private IReadOnlyList<Tuple<AnimalType, DietType>> CreateDietKeys()
        {
            var dietList = new List<Tuple<AnimalType, DietType>>()
            {
                new (AnimalType.BeefCow, DietType.LowEnergyAndProtein),
                new (AnimalType.BeefCow, DietType.MediumEnergyAndProtein)
            };

            return dietList;
        }

        #endregion
    }
}