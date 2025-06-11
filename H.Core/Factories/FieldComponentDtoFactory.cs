using AutoMapper;
using Avalonia.Markup.Xaml.Templates;
using H.Core.Models.LandManagement.Fields;

namespace H.Core.Factories;

public class FieldComponentDtoFactory : IFieldComponentDtoFactory
{
    #region Fields

    private readonly IMapper _fieldComponentMapper;

    #endregion

    #region Constructors

    public FieldComponentDtoFactory()
    {
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

    public IFieldComponentDto Create(FieldSystemComponent template)
    {
        var fieldComponentDto = new FieldSystemComponentDto();

        _fieldComponentMapper.Map(template, fieldComponentDto);

        return fieldComponentDto;
    }

    #endregion

    #region Private Methods

    

    #endregion
}