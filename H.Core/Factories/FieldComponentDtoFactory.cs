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

    /// <summary>
    /// Create a new instance that is based on the state of an existing <see cref="FieldSystemComponent"/>. This method is used to create a
    /// new instance of a <see cref="FieldSystemComponentDto"/> that will be bound to a view.
    /// </summary>
    /// <param name="template">The <see cref="FieldSystemComponent"/> that will be used to provide default values for the new <see cref="FieldSystemComponentDto"/> instance</param>
    /// <returns></returns>
    public IFieldComponentDto Create(FieldSystemComponent template)
    {
        var fieldComponentDto = new FieldSystemComponentDto();

        // Create a copy of the template
        _fieldComponentToDtoMapper.Map(template, fieldComponentDto);

        // All numerical values are stored internally as metric values
        var fieldComponentDtoPropertyConverter = new PropertyConverter<IFieldComponentDto>(fieldComponentDto);

        // Get all properties that might need to be converted to imperial units before being shown to the user
        foreach (var property in fieldComponentDtoPropertyConverter.PropertyInfos)
        {
            // Convert the value from metric to imperial as needed
            var bindingValue = fieldComponentDtoPropertyConverter.GetBindingValueFromSystem(property, _unitsOfMeasurementCalculator.GetUnitsOfMeasurement());

            // Set the value of the property before displaying to the user
            property.SetValue(fieldComponentDto, bindingValue);
        }

        this.BuildCropDtoCollection(template, fieldComponentDto);

        return fieldComponentDto;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Create copies of all the <see cref="CropViewItem"/> in a <see cref="FieldSystemComponent"/> and add corresponding <see cref="CropDto"/> instances to the <see cref="FieldSystemComponentDto"/>
    /// </summary>
    /// <param name="fieldSystemComponent">The </param>
    /// <param name="fieldComponentDto"></param>
    private void BuildCropDtoCollection(FieldSystemComponent fieldSystemComponent, IFieldComponentDto fieldComponentDto)
    {
        fieldComponentDto.CropDtos.Clear();

        foreach (var cropViewItem in fieldSystemComponent.CropViewItems)
        {
            var dto = _cropDtoFactory.Create(template: cropViewItem);

            fieldComponentDto.CropDtos.Add(dto);
        }
    }

    #endregion
}