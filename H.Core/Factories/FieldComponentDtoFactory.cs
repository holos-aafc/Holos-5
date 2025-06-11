using AutoMapper;
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
        var fieldComponentDtoMapperConfiguration = new MapperConfiguration(configuration => { configuration.CreateMap<FieldSystemComponentDto, FieldSystemComponent>(); });

        _fieldComponentMapper = fieldComponentDtoMapperConfiguration.CreateMapper();
    }

    #endregion

    #region Public Methods

    public IFieldComponentDto Create()
    {
        throw new NotImplementedException();
    }

    public IFieldComponentDto Create(FieldSystemComponent template)
    {
        var fieldComponentDto = new FieldSystemComponentDto();

        _fieldComponentMapper.Map(template, fieldComponentDto);

        return fieldComponentDto;
    }

    #endregion
}