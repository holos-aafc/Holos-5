using AutoMapper;
using Avalonia.Markup.Xaml.Templates;
using H.Core.Models.LandManagement.Fields;

namespace H.Core.Factories;

/// <summary>
/// A class used to create new <see cref="FieldSystemComponentDto"/> isntances. The class will provide basic initialization of a new instance before returning the result to the caller.
/// </summary>
public class FieldComponentDtoFactory : IFieldComponentDtoFactory
{
    #region Fields

    private readonly IMapper _fieldComponentMapper;
    private readonly ICropDtoFactory _cropDtoFactory;

    #endregion

    #region Constructors

    public FieldComponentDtoFactory(ICropDtoFactory cropDtoFactory)
    {
        if (cropDtoFactory != null)
        {
            _cropDtoFactory = cropDtoFactory; 
        }
        else
        {
            throw new ArgumentNullException(nameof(cropDtoFactory));
        }
        
        var fieldComponentDtoMapperConfiguration = new MapperConfiguration(configuration => { configuration.CreateMap<FieldSystemComponent, FieldSystemComponentDto>(); });

        _fieldComponentMapper = fieldComponentDtoMapperConfiguration.CreateMapper();
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
        _fieldComponentMapper.Map(template, fieldComponentDto);

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