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
    private readonly IMapper _fieldComponentToDtoMapper;

    private readonly IMapper _cropDtoToCropViewItemMapper;
    private readonly IMapper _cropViewItemToCropDtoMapper;

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
        var fieldComponentDtoMapperConfiguration = new MapperConfiguration(configuration => { configuration.CreateMap<FieldSystemComponent, FieldSystemComponentDto>(); });

        var cropDtoToCropViewItemMapperConfiguration = new MapperConfiguration(configuration => { configuration.CreateMap<ICropDto, CropViewItem>(); });
        var cropViewItemToCropDtoMapperConfiguration = new MapperConfiguration(configuration => { configuration.CreateMap<CropViewItem, ICropDto>(); });

        _fieldDtoToComponentMapper = fieldDtoToComponentMapperConfiguration.CreateMapper();
        _fieldComponentToDtoMapper = fieldComponentDtoMapperConfiguration.CreateMapper();

        _cropDtoToCropViewItemMapper = cropDtoToCropViewItemMapperConfiguration.CreateMapper();
        _cropViewItemToCropDtoMapper = cropViewItemToCropDtoMapperConfiguration.CreateMapper();
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
            fieldDto = TransferToFieldComponentDto(template: template);
        }
        else
        {
            fieldDto = _fieldComponentDtoFactory.Create();
            template.IsInitialized = true;
        }

        return fieldDto;
    }

    public IFieldComponentDto CreateFieldDto(IFieldComponentDto template)
    {
        return _fieldComponentDtoFactory.CreateFieldDto(template);
    }

    public ICropDto CreateCropDto()
    {
        return _cropDtoFactory.CreateCropDto();
    }

    public ICropDto CreateCropDto(CropViewItem template)
    {
        return _cropDtoFactory.CreateCropDto(template);
    }

    public ICropDto CreateCropDto(ICropDto template)
    {
        return _cropDtoFactory.CreateCropDto(template);
    }

    public ICropDto TransferCropViewItemToCropDto(CropViewItem cropViewItem)
    {
        var dto = new CropDto();

        // Create a copy of the view item by copying all properties into the DTO
        _cropViewItemToCropDtoMapper.Map(cropViewItem, dto);

        // All numerical values are stored internally as metric values
        var cropDtoPropertyConverter = new PropertyConverter<ICropDto>(dto);

        // Get all properties that might need to be converted to imperial units before being shown to the user
        foreach (var propertyInfo in cropDtoPropertyConverter.PropertyInfos)
        {
            // Convert the value from metric to imperial as needed. Note the converter won't convert anything if the display is in metric units
            var bindingValue = cropDtoPropertyConverter.GetBindingValueFromSystem(propertyInfo, _unitsOfMeasurementCalculator.GetUnitsOfMeasurement());

            // Set the value of the property before displaying to the user
            propertyInfo.SetValue(dto, bindingValue);
        }

        return dto;
    }

    public CropViewItem TransferCropDtoToSystem(ICropDto cropDto, CropViewItem cropViewItem)
    {
        // Create a copy of the DTO since we don't want to change values on the original that is still bound to the GUI
        var copy = _cropDtoFactory.CreateCropDto(cropDto);

        // All numerical values are stored internally as metric values
        var cropViewItemPropertyConverter = new PropertyConverter<ICropDto>(copy);

        // Get all properties that might need to be converted to imperial units before being shown to the user
        foreach (var property in cropViewItemPropertyConverter.PropertyInfos)
        {
            // Convert the value from imperial to metric as needed (no conversion will occur if display is using metric)
            var bindingValue = cropViewItemPropertyConverter.GetSystemValueFromBinding(property, _unitsOfMeasurementCalculator.GetUnitsOfMeasurement());

            // Set the value on the copy of the DTO
            property.SetValue(copy, bindingValue);
        }

        // Map values from the copy of the DTO to the internal system object
        _cropDtoToCropViewItemMapper.Map(copy, cropViewItem);

        return cropViewItem;
    }

    public FieldSystemComponent TransferFieldDtoToSystem(IFieldComponentDto fieldComponentDto, FieldSystemComponent fieldSystemComponent)
    {
        // Create a copy of the DTO since we don't want to change values on the original that is still bound to the GUI
        var copy = _fieldComponentDtoFactory.CreateFieldDto(fieldComponentDto);

        // All numerical values are stored internally as metric values
        var fieldComponentDtoPropertyConverter = new PropertyConverter<IFieldComponentDto>(copy);

        // Get all properties that might need to be converted
        foreach (var property in fieldComponentDtoPropertyConverter.PropertyInfos)
        {
            // Convert the value from imperial to metric as needed (no conversion will occur if display is using metric)
            var bindingValue = fieldComponentDtoPropertyConverter.GetSystemValueFromBinding(property, _unitsOfMeasurementCalculator.GetUnitsOfMeasurement());

            // Set the value on the copy of the DTO
            property.SetValue(copy, bindingValue);
        }

        // Map values from the copy of the DTO to the internal system object
        _fieldDtoToComponentMapper.Map(copy, fieldSystemComponent);

        return fieldSystemComponent;
    }

    public IFieldComponentDto TransferToFieldComponentDto(FieldSystemComponent template)
    {
        var fieldComponentDto = new FieldSystemComponentDto();

        // Create a copy of the field component by copying all properties into the DTO
        _fieldComponentToDtoMapper.Map(template, fieldComponentDto);

        // All numerical values are stored internally as metric values
        var fieldComponentDtoPropertyConverter = new PropertyConverter<IFieldComponentDto>(fieldComponentDto);

        // Get all properties that might need to be converted to imperial units before being shown to the user
        foreach (var property in fieldComponentDtoPropertyConverter.PropertyInfos)
        {
            // Convert the value from metric to imperial as needed. Note the converter won't convert anything if the display is in metric units
            var bindingValue = fieldComponentDtoPropertyConverter.GetBindingValueFromSystem(property, _unitsOfMeasurementCalculator.GetUnitsOfMeasurement());

            // Set the value of the property before displaying to the user
            property.SetValue(fieldComponentDto, bindingValue);
        }

        this.BuildCropDtoCollection(template, fieldComponentDto);

        return fieldComponentDto;
    }

    public void BuildCropDtoCollection(FieldSystemComponent fieldSystemComponent, IFieldComponentDto fieldComponentDto)
    {
        fieldComponentDto.CropDtos.Clear();

        foreach (var cropViewItem in fieldSystemComponent.CropViewItems)
        {
            var dto = _cropDtoFactory.CreateCropDto(template: cropViewItem);

            fieldComponentDto.CropDtos.Add(dto);
        }
    }

    #endregion

    #region Private Methods

    #endregion
}