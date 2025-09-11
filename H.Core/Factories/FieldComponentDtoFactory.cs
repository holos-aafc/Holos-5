using AutoMapper;
using Avalonia.Markup.Xaml.Templates;
using H.Core.Calculators.UnitsOfMeasurement;
using H.Core.Converters;
using H.Core.Mappers;
using H.Core.Models.LandManagement.Fields;
using Prism.Ioc;

namespace H.Core.Factories;

/// <summary>
/// A class used to create new <see cref="FieldSystemComponentDto"/> instances. The class will provide basic initialization of a new instance before returning the result to the caller.
/// </summary>
public class FieldComponentDtoFactory : IFieldComponentDtoFactory
{
    #region Fields

    private readonly IMapper _fieldComponentToDtoMapper;
    private readonly IMapper _fieldDtoToDtoMapper;

    private readonly ICropFactory _cropFactory;
    private PropertyConverter<IFieldComponentDto> _fieldComponentDtoPropertyConverter;
    private readonly IUnitsOfMeasurementCalculator _unitsOfMeasurementCalculator;

    #endregion

    #region Constructors

    public FieldComponentDtoFactory(ICropFactory cropFactory, IUnitsOfMeasurementCalculator unitsOfMeasurementCalculator, IContainerProvider containerProvider)
    {
        if (containerProvider == null)
        {
            throw new ArgumentNullException(nameof(containerProvider));
        }

        if (unitsOfMeasurementCalculator != null)
        {
            _unitsOfMeasurementCalculator = unitsOfMeasurementCalculator;
        }
        else
        {
            throw new ArgumentNullException(nameof(unitsOfMeasurementCalculator));
        }

        if (cropFactory != null)
        {
            _cropFactory = cropFactory;
        }
        else
        {
            throw new ArgumentNullException(nameof(cropFactory));
        }

        _fieldComponentToDtoMapper = containerProvider.Resolve<IMapper>(nameof(FieldComponentToDtoMapper));
        _fieldDtoToDtoMapper = containerProvider.Resolve<IMapper>(nameof(FieldDtoToFieldDtoMapper));
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Create a new instance with no additional configuration to a default instance.
    /// </summary>
    public IFieldComponentDto Create()
    {
        return new FieldSystemComponentDto();
    }

    public IFieldComponentDto CreateFieldDto(IFieldComponentDto template)
    {
        var fieldComponentDto = new FieldSystemComponentDto();

        _fieldDtoToDtoMapper.Map(template, fieldComponentDto);

        return fieldComponentDto;
    }

    #endregion

    #region Private Methods

    #endregion
}