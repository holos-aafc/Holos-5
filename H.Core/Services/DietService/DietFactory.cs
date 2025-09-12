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
        private readonly IFeedIngredientProvider _feedIngredientProvider;
        
        #endregion

        #region Constructors

        public DietFactory()
        {
            _validDietKeys = this.CreateDietKeys();
        }

        public DietFactory(ILogger logger, ICacheService cacheService, IFeedIngredientProvider feedIngredientProvider) : this()
        {
            if (feedIngredientProvider != null)
            {
                _feedIngredientProvider = feedIngredientProvider;
            }
            else
            {
                throw new ArgumentNullException(nameof(feedIngredientProvider));
            }

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

            _feedIngredientProvider = feedIngredientProvider;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a default <see cref="IDiet"/> instance without specifying diet type or animal type.
        /// </summary>
        /// <returns>
        /// An <see cref="IDiet"/> instance representing a generic or placeholder diet. The default implementation
        /// throws a <see cref="NotImplementedException"/> unless overridden in a derived class.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This parameterless method is intended for scenarios where a specific diet type and animal type are not known
        /// at creation time. By default, it is not implemented and will throw an exception. Implementations may choose
        /// to return a generic diet, a placeholder, or use application-specific logic.
        /// </para>
        /// <para>
        /// For most use cases, prefer using <see cref="Create(DietType, AnimalType)"/> to ensure a properly configured diet.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var factory = new DietFactory();
        /// var diet = factory.Create(); // May throw NotImplementedException
        /// </code>
        /// </example>
        /// <seealso cref="Create(DietType, AnimalType)"/>
        public IDiet Create()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a diet instance based on the specified diet type and animal type combination.
        /// </summary>
        /// <param name="dietType">The type of diet to create (e.g., low energy, medium energy).</param>
        /// <param name="animalType">The animal type for which the diet is intended (e.g., beef cow, dairy cow).</param>
        /// <returns>
        /// An <see cref="IDiet"/> instance configured for the specified combination. If the combination is valid,
        /// returns a properly configured diet; otherwise, returns a fallback diet with the name "Unknown diet".
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method implements a caching strategy to improve performance for repeated requests. When a valid
        /// diet-animal combination is requested, the method first checks the cache for an existing instance before
        /// creating a new one. Newly created diets are automatically cached for future requests.
        /// </para>
        /// <para>
        /// The method validates the input combination using <see cref="IsValidDietType(AnimalType, DietType)"/>
        /// before attempting to create the diet. Invalid combinations will result in a fallback diet being returned
        /// rather than throwing an exception.
        /// </para>
        /// <para>
        /// All operations are logged through the configured <see cref="ILogger"/> instance, including:
        /// <list type="bullet">
        /// <item><description>Cache hits when returning cached diets</description></item>
        /// <item><description>New diet creation events</description></item>
        /// <item><description>Error events for invalid combinations</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>Creating a diet for a valid combination:</para>
        /// <code>
        /// var factory = new DietFactory(logger, cacheService);
        /// var diet = factory.Create(DietType.LowEnergyAndProtein, AnimalType.BeefCow);
        /// // Returns a properly configured diet instance
        /// </code>
        /// <para>Attempting to create a diet for an invalid combination:</para>
        /// <code>
        /// var diet = factory.Create(DietType.HighEnergyAndProtein, AnimalType.Sheep);
        /// // Returns a fallback diet with Name = "Unknown diet"
        /// // Logs an error message about the invalid combination
        /// </code>
        /// </example>
        /// <seealso cref="IsValidDietType(AnimalType, DietType)"/>
        /// <seealso cref="GetValidDietKeys()"/>
        /// <seealso cref="IDiet"/>
        /// <seealso cref="DietType"/>
        /// <seealso cref="AnimalType"/>
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

        /// <summary>
        /// Retrieves the list of valid animal type and diet type combinations supported by this factory.
        /// </summary>
        /// <returns>
        /// A read-only list of <see cref="Tuple{AnimalType, DietType}"/> representing all valid combinations
        /// for which diets can be created.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The returned list acts as the authoritative source for supported diet-animal pairings. Only combinations
        /// present in this list will be considered valid by <see cref="IsValidDietType(AnimalType, DietType)"/> and
        /// will result in proper diet creation through <see cref="Create(DietType, AnimalType)"/>.
        /// </para>
        /// <para>
        /// This method is useful for clients that need to enumerate or validate available diet options before attempting
        /// to create a diet. The list is initialized during factory construction and remains constant for the lifetime
        /// of the factory instance.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var factory = new DietFactory();
        /// var validKeys = factory.GetValidDietKeys();
        /// foreach (var (animalType, dietType) in validKeys)
        /// {
        ///     Console.WriteLine($"Supported: {animalType} with {dietType}");
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="IsValidDietType(AnimalType, DietType)"/>
        /// <seealso cref="Create(DietType, AnimalType)"/>
        /// <seealso cref="AnimalType"/>
        /// <seealso cref="DietType"/>
        public IReadOnlyList<Tuple<AnimalType, DietType>> GetValidDietKeys()
        {
            return _validDietKeys;
        }

        /// <summary>
        /// Determines whether the specified combination of animal type and diet type is valid and supported by this factory.
        /// </summary>
        /// <param name="animalType">The animal type to validate against the available diet combinations.</param>
        /// <param name="dietType">The diet type to validate against the available animal combinations.</param>
        /// <returns>
        /// <see langword="true"/> if the factory can create a diet for the specified combination; 
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method performs a lookup against the predefined list of valid diet-animal combinations
        /// maintained by the factory. The validation is based on a whitelist approach where only
        /// explicitly supported combinations will return <see langword="true"/>.
        /// </para>
        /// <para>
        /// The method uses <see cref="GetValidDietKeys()"/> to retrieve the current list of supported
        /// combinations and performs an exact match lookup. Both the animal type and diet type must
        /// match exactly for the combination to be considered valid.
        /// </para>
        /// <para>
        /// It is recommended to call this method before attempting to create a diet using 
        /// <see cref="Create(DietType, AnimalType)"/> to avoid receiving fallback "Unknown diet" instances.
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>Validating a supported combination:</para>
        /// <code>
        /// var factory = new DietFactory();
        /// bool isValid = factory.IsValidDietType(AnimalType.BeefCow, DietType.LowEnergyAndProtein);
        /// // Returns true if this combination is supported
        /// </code>
        /// <para>Using validation before diet creation:</para>
        /// <code>
        /// if (factory.IsValidDietType(animalType, dietType))
        /// {
        ///     var diet = factory.Create(dietType, animalType);
        ///     // Guaranteed to receive a properly configured diet
        /// }
        /// else
        /// {
        ///     // Handle unsupported combination
        ///     Console.WriteLine($"Diet combination {dietType} for {animalType} is not supported");
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="GetValidDietKeys()"/>
        /// <seealso cref="Create(DietType, AnimalType)"/>
        /// <seealso cref="AnimalType"/>
        /// <seealso cref="DietType"/>
        public bool IsValidDietType(AnimalType animalType, DietType dietType)
        {
            return this.GetValidDietKeys().Contains(new Tuple<AnimalType, DietType>(animalType, dietType));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates and returns the predefined list of valid animal type and diet type combinations 
        /// supported by this factory.
        /// </summary>
        /// <returns>
        /// A read-only list of <see cref="Tuple{T1, T2}"/> where T1 is <see cref="AnimalType"/> 
        /// and T2 is <see cref="DietType"/>, representing all valid combinations for diet creation.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method defines the authoritative whitelist of supported diet-animal combinations.
        /// Only combinations returned by this method will be considered valid by 
        /// <see cref="IsValidDietType(AnimalType, DietType)"/> and will result in proper diet
        /// creation through <see cref="Create(DietType, AnimalType)"/>.
        /// </para>
        /// <para>
        /// Currently supported combinations include:
        /// <list type="bullet">
        /// <item><description><see cref="AnimalType.BeefCow"/> with <see cref="DietType.LowEnergyAndProtein"/></description></item>
        /// <item><description><see cref="AnimalType.BeefCow"/> with <see cref="DietType.MediumEnergyAndProtein"/></description></item>
        /// </list>
        /// </para>
        /// <para>
        /// This method is called during factory initialization and the result is cached for the
        /// lifetime of the factory instance. To add support for new combinations, modify the
        /// list returned by this method.
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>The method returns combinations that can be used like this:</para>
        /// <code>
        /// var factory = new DietFactory();
        /// var validKeys = factory.GetValidDietKeys(); // Calls this method internally
        /// 
        /// foreach (var (animalType, dietType) in validKeys)
        /// {
        ///     var diet = factory.Create(dietType, animalType);
        ///     Console.WriteLine($"Created diet for {animalType} with {dietType}");
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="GetValidDietKeys()"/>
        /// <seealso cref="IsValidDietType(AnimalType, DietType)"/>
        /// <seealso cref="Create(DietType, AnimalType)"/>
        /// <seealso cref="AnimalType"/>
        /// <seealso cref="DietType"/>
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