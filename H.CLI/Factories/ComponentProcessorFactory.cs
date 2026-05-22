using H.CLI.Interfaces;
using H.CLI.Processors;
using System;

namespace H.CLI.Factories
{
    public class ComponentProcessorFactory
    {
        #region Fields

        private readonly FieldProcessor _fieldProcessor;
        private readonly ShelterbeltProcessor _shelterbeltProcessor;

        #endregion

        #region Constructors

        public ComponentProcessorFactory(FieldProcessor fieldProcessor, ShelterbeltProcessor shelterbeltProcessor)
        {
            _fieldProcessor = fieldProcessor ?? throw new ArgumentNullException(nameof(fieldProcessor));
            _shelterbeltProcessor = shelterbeltProcessor ?? throw new ArgumentNullException(nameof(shelterbeltProcessor));
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Based on the type of Component in our farm's list of components, return the appropriate concrete Processor
        /// </summary>
        /// <param name="componentType"></param>
        /// <returns></returns>
        public IProcessor GetComponentProcessor(Type componentType)
        {
            switch (componentType.Name.ToUpper())
            {
                case "SHELTERBELTCOMPONENT":
                    return _shelterbeltProcessor;
                case "FIELDSYSTEMCOMPONENT":
                    return _fieldProcessor;
                default:
                    return _shelterbeltProcessor;
            }
        }
        #endregion
    }
}
