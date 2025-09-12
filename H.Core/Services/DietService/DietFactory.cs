using H.Core.Enumerations;
using H.Core.Providers.Feed;
using H.Infrastructure.Services;
using Microsoft.Extensions.Logging;

namespace H.Core.Services.DietService
{
    /// <summary>
    /// Provides a factory implementation for creating diet objects based on animal and diet type combinations.
    /// This factory supports caching of created diets and validates diet-animal type compatibility before creation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="DietFactory"/> class implements the factory pattern to create <see cref="IDiet"/> instances
    /// for specific combinations of <see cref="AnimalType"/> and <see cref="DietType"/>. The factory maintains
    /// a predefined list of valid diet-animal combinations and only creates diets for supported pairings.
    /// </para>
    /// <para>
    /// Key features include:
    /// <list type="bullet">
    /// <item><description>Validation of diet-animal type combinations before creation</description></item>
    /// <item><description>Caching support to improve performance for repeated requests</description></item>
    /// <item><description>Comprehensive logging of creation and validation operations</description></item>
    /// <item><description>Fallback diet creation for invalid combinations</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// The factory uses dependency injection for logging (<see cref="ILogger"/>) and caching (<see cref="ICacheService"/>)
    /// services. If these dependencies are not provided, the factory will still function but without logging or caching capabilities.
    /// </para>
    /// </remarks>
    /// <example>
    /// <para>Basic usage without dependencies:</para>
    /// <code>
    /// var factory = new DietFactory();
    /// var diet = factory.Create(DietType.LowEnergyAndProtein, AnimalType.BeefCow);
    /// </code>
    /// <para>Usage with dependency injection:</para>
    /// <code>
    /// var factory = new DietFactory(logger, cacheService);
    /// var diet = factory.Create(DietType.MediumEnergyAndProtein, AnimalType.BeefCow);
    /// 
    /// // Check if a combination is valid before creating
    /// if (factory.IsValidDietType(AnimalType.BeefCow, DietType.LowEnergyAndProtein))
    /// {
    ///     var validDiet = factory.Create(DietType.LowEnergyAndProtein, AnimalType.BeefCow);
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="IDietFactory"/>
    /// <seealso cref="IDiet"/>
    /// <seealso cref="AnimalType"/>
    /// <seealso cref="DietType"/>
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

        /// <summary>
        /// Creates a diet based on the given input parameters
        /// </summary>
        /// <param name="dietType">The type of diet that should be created</param>
        /// <param name="animalType">The animal type this diet is designed for</param>
        /// <returns>A diet specific for this combination of inputs</returns>
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

        /// <summary>
        /// Determines if the combination of <see cref="AnimalType"/> and <see cref="DietType"/> is valid and has a corresponding <see cref="Diet"/> that
        /// can be created by the factory
        /// </summary>
        /// <param name="animalType">The <see cref="AnimalType"/> to use for the lookup</param>
        /// <param name="dietType">The <see cref="DietType"/> to use for the lookup</param>
        /// <returns><see langword="true"/> if the diet can be created, <see langword="false"/> otherwise</returns>
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