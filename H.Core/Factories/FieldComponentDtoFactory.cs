using AutoMapper;
using Avalonia.Markup.Xaml.Templates;
using H.Core.Models.LandManagement.Fields;

namespace H.Core.Factories;

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

    public IFieldComponentDto Create()
    {
        var fieldComponentDto = new FieldSystemComponentDto();
        fieldComponentDto.Name = "Field_" + DateTime.Now;

        return fieldComponentDto;
    }

    public IFieldComponentDto Create(FieldSystemComponent fieldSystemComponent)
    {
        var fieldComponentDto = new FieldSystemComponentDto();

        _fieldComponentMapper.Map(fieldSystemComponent, fieldComponentDto);

        this.BuildCropDtoCollection(fieldSystemComponent, fieldComponentDto);

        return fieldComponentDto;
    }

    #endregion

    #region Private Methods

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