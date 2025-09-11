using AutoMapper;
using H.Core.Factories;

namespace H.Core.Mappers;

public class FieldDtoToFieldDtoMapper : Profile
{
    public FieldDtoToFieldDtoMapper()
    {
        CreateMap<FieldSystemComponentDto, FieldSystemComponentDto>();
    }
}