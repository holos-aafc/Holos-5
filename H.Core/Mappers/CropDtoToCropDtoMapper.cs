using AutoMapper;
using H.Core.Factories;
using H.Core.Models.LandManagement.Fields;

namespace H.Core.Mappers;

public class CropDtoToCropDtoMapper : Profile
{
    public CropDtoToCropDtoMapper()
    {
        CreateMap<ICropDto, ICropDto>();
    }
}