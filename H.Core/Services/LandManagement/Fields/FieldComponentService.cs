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

    #endregion

    #region Constructors

    public FieldComponentService(IFieldComponentDtoFactory fieldComponentDtoFactory, ICropDtoFactory cropDtoFactory)
    {
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

    #endregion

    #region Private Methods

    #endregion

    public ICropDto CreateCropDto()
    {
        return _cropDtoFactory.CreateCropDto();
    }

    public ICropDto Create(CropViewItem template)
    {
        return _cropDtoFactory.Create(template);
    }
}