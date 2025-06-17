using AutoMapper;
using H.Core.Calculators.UnitsOfMeasurement;
using H.Core.Converters;
using H.Core.Factories;
using H.Core.Models;
using H.Core.Models.LandManagement.Fields;

namespace H.Core.Services.LandManagement.Fields;

/// <summary>
/// A general service class to assist with various operations needing to be done on a <see cref="FieldSystemComponent"/> or <see cref="FieldSystemComponentDto"/>
/// </summary>
public class FieldComponentService : IFieldComponentService
{
    #region Fields
    
    private readonly IFieldComponentDtoFactory _fieldComponentDtoFactory;
    private readonly ICropDtoFactory _cropDtoFactory;
    private readonly IUnitsOfMeasurementCalculator _unitsOfMeasurementCalculator;
    private readonly IMapper _fieldDtoToComponentMapper;

    #endregion

    #region Constructors

    public FieldComponentService(IFieldComponentDtoFactory fieldComponentDtoFactory, ICropDtoFactory cropDtoFactory, IUnitsOfMeasurementCalculator unitsOfMeasurementCalculator)
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

        if (fieldComponentDtoFactory != null)
        {
            _fieldComponentDtoFactory = fieldComponentDtoFactory;
        }
        else
        {
            throw new ArgumentNullException(nameof(fieldComponentDtoFactory));
        }

        var fieldDtoToComponentMapperConfiguration = new MapperConfiguration(configuration => { configuration.CreateMap<FieldSystemComponentDto, FieldSystemComponent>(); });

        _fieldDtoToComponentMapper = fieldDtoToComponentMapperConfiguration.CreateMapper();
    }

    #endregion

    #region Public Methods

    public void InitializeFieldSystemComponent(Farm farm, FieldSystemComponent fieldSystemComponent)
    {
        if (fieldSystemComponent.IsInitialized)
        {
            // The field has already been initialized - do not overwrite with default values
            return;
        }

        fieldSystemComponent.Name = this.GetUniqueFieldName(farm.FieldSystemComponents);

        fieldSystemComponent.IsInitialized = true;
    }

    public void InitializeCropDto(IFieldComponentDto fieldComponentDto, ICropDto cropDto)
    {
        cropDto.Year = this.GetNextCropYear(fieldComponentDto);

        fieldComponentDto.CropDtos.Add(cropDto);
    }

    public string GetUniqueFieldName(IEnumerable<FieldSystemComponent> components)
    {
        var i = 1;
        var fieldSystemComponents = components;

        var proposedName = string.Format(Properties.Resources.InterpolatedFieldNumber, i);

        // While proposedName isn't unique, create a unique name for this component so we don't have two or more components with same name
        while (fieldSystemComponents.Any(x => x.Name == proposedName))
        {
            proposedName = string.Format(Properties.Resources.InterpolatedFieldNumber, ++i);
        }

        return proposedName;
    }

    public int GetNextCropYear(IFieldComponentDto fieldComponentDto)
    {
        var result = DateTime.Now.Year;

        if (fieldComponentDto.CropDtos.Any())
        {
            result = fieldComponentDto.CropDtos.Min(dto => dto.Year) - 1;
        }

        return result;
    }

    public void ResetAllYears(IEnumerable<ICropDto> cropDtos)
    {
        if (cropDtos.Any())
        {
            var maximumYear = cropDtos.Max(dto => dto.Year);

            var orderedDtos = cropDtos.OrderByDescending(dto => dto.Year);
            for (int i = 0; i < cropDtos.Count(); i++)
            {
                var dto = orderedDtos.ElementAt(i);
                dto.Year = maximumYear - i;
            }
        }
    }

    public IFieldComponentDto Create()
    {
        return _fieldComponentDtoFactory.Create();
    }

    public IFieldComponentDto Create(FieldSystemComponent template)
    {
        IFieldComponentDto fieldDto;

        if (template.IsInitialized)
        {
            fieldDto = _fieldComponentDtoFactory.Create(template: template);
        }
        else
        {
            fieldDto = _fieldComponentDtoFactory.Create();
            template.IsInitialized = true;
        }

        return fieldDto;
    }

    public IFieldComponentDto Create(IFieldComponentDto template)
    {
        return _fieldComponentDtoFactory.Create(template);
    }

    public ICropDto CreateCropDto()
    {
        return _cropDtoFactory.CreateCropDto();
    }

    public ICropDto Create(CropViewItem template)
    {
        return _cropDtoFactory.Create(template);
    }

    /// <summary>
    /// The view is always bound to a DTO object. Once to user enters values the DTO needs to have its values converted to metric since Holos stores all values in metric units
    /// internally. This method takes in a DTO, converts values to the correct units of measurement, as assigns those converted values to the system/domain object.
    /// </summary>
    /// <param name="fieldComponentDto">The DTO that is bound to the GUI</param>
    /// <param name="fieldSystemComponent">The internal system object</param>
    /// <returns>The <see cref="FieldSystemComponent"/> once the converted values have been assigned</returns>
    public FieldSystemComponent TransferToSystem(IFieldComponentDto fieldComponentDto, FieldSystemComponent fieldSystemComponent)
    {
        // Create a copy of the DTO since we don't want to change values on the original that is still bound to the GUI
        var copy = _fieldComponentDtoFactory.Create(fieldComponentDto);

        // All numerical values are stored internally as metric values
        var fieldComponentDtoPropertyConverter = new PropertyConverter<IFieldComponentDto>(copy);

        // Get all properties that might need to be converted to imperial units before being shown to the user
        foreach (var property in fieldComponentDtoPropertyConverter.PropertyInfos)
        {
            // Convert the value from imperial to metric as needed (no conversion will occur if display is using metric)
            var bindingValue = fieldComponentDtoPropertyConverter.GetSystemValueFromBinding(property, _unitsOfMeasurementCalculator.GetUnitsOfMeasurement());

            // Set the value on the copy of the DTO
            property.SetValue(copy, bindingValue);
        }

        // Map value from the copy of the DTO to the internal system object
        _fieldDtoToComponentMapper.Map(copy, fieldSystemComponent);

        return fieldSystemComponent;
    }

    #endregion

    #region Private Methods

    #endregion


}