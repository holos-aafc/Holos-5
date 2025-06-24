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

    public void Initialize(Farm farm, FieldSystemComponent fieldSystemComponent)
    {
        if (fieldSystemComponent.IsInitialized)
        {
            return;
        }

        fieldSystemComponent.Name = this.GetUniqueFieldName(farm.FieldSystemComponents);

        fieldSystemComponent.IsInitialized = true;
    }

    public void Initialize(IFieldComponentDto fieldComponentDto, ICropDto cropDto)
    {
        cropDto.Year = this.GetNextCropYear(fieldComponentDto);
    }

    /// <summary>
    /// Crates a unique component name when adding a field to the farm
    /// </summary>
    /// <param name="components"></param>
    /// <returns></returns>
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

    /// <summary>
    /// When adding a new crop to the field, the year must be the next in order so that all years of the field history are consecutive.
    /// </summary>
    /// <param name="fieldComponentDto">The field containing the crops</param>
    /// <returns>The next consecutive year to should be used</returns>
    public int GetNextCropYear(IFieldComponentDto fieldComponentDto)
    {
        var result = DateTime.Now.Year;

        if (fieldComponentDto.CropDtos.Any())
        {
            result = fieldComponentDto.CropDtos.Min(dto => dto.Year) - 1;
        }

        return result;
    }

    /// <summary>
    /// All years in a collection of crops must be consecutive with no years missing. If a crop is removed, ensure all years are represented from start to finish of collection.
    /// </summary>
    /// <param name="cropDtos">The list of crops and associated years representing the history of the field</param>
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

    public IFieldComponentDto Create(FieldSystemComponent fieldSystemComponent)
    {
        IFieldComponentDto fieldDto;

        if (fieldSystemComponent.IsInitialized)
        {
            fieldDto = _fieldComponentDtoFactory.Create(fieldSystemComponent: fieldSystemComponent);
        }
        else
        {
            fieldDto = _fieldComponentDtoFactory.Create();
            fieldSystemComponent.IsInitialized = true;
        }

        return fieldDto;
    }

    #endregion

    #region Private Methods

    #endregion
}