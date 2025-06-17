using AutoMapper;
using Avalonia.Markup.Xaml.Templates;
using H.Core.Calculators.UnitsOfMeasurement;
using H.Core.Converters;
using H.Core.Models.LandManagement.Fields;

namespace H.Core.Factories;

/// <summary>
/// A class used to create new <see cref="FieldSystemComponentDto"/> isntances. The class will provide basic initialization of a new instance before returning the result to the caller.
/// </summary>
public class FieldComponentDtoFactory : IFieldComponentDtoFactory
{
    #region Fields

    private readonly IMapper _fieldComponentToDtoMapper;
    private readonly IMapper _fieldDtoToDtoMapper;

    private readonly ICropDtoFactory _cropDtoFactory;
    private PropertyConverter<IFieldComponentDto> _fieldComponentDtoPropertyConverter;
    private readonly IUnitsOfMeasurementCalculator _unitsOfMeasurementCalculator;

    #endregion

    #region Constructors

    public FieldComponentDtoFactory(ICropDtoFactory cropDtoFactory, IUnitsOfMeasurementCalculator unitsOfMeasurementCalculator)
    {
        if (unitsOfMeasurementCalculator != null)
        {
            _unitsOfMeasurementCalculator = unitsOfMeasurementCalculator;
        }
        else
        {
            throw new ArgumentNullException(nameof(unitsOfMeasurementCalculator));
        }

        if (cropDtoFactory != null)
        {
            _cropDtoFactory = cropDtoFactory;
        }
        else
        {
            throw new ArgumentNullException(nameof(cropDtoFactory));
        }

        var fieldComponentDtoMapperConfiguration = new MapperConfiguration(configuration => { configuration.CreateMap<FieldSystemComponent, FieldSystemComponentDto>(); });
        var fieldDtoToDtoMapperConfiguration = new MapperConfiguration(configuration => { configuration.CreateMap<FieldSystemComponentDto, FieldSystemComponentDto>(); });

        _fieldComponentToDtoMapper = fieldComponentDtoMapperConfiguration.CreateMapper();
        _fieldDtoToDtoMapper = fieldDtoToDtoMapperConfiguration.CreateMapper();
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

    public IFieldComponentDto Create(IFieldComponentDto template)
    {
        var fieldComponentDto = new FieldSystemComponentDto();

        _fieldDtoToDtoMapper.Map(template, fieldComponentDto);

        return fieldComponentDto;
    }



    #endregion

    #region Private Methods



    #endregion
}