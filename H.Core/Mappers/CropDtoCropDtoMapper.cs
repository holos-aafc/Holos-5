using AutoMapper;
using H.Core.Factories;
using H.Core.Models.LandManagement.Fields;

namespace H.Core.Mappers;

public class CropDToCropDtoMapper : Profile
{
    public CropDToCropDtoMapper()
    {
        CreateMap<CropViewItem, CropDto>();
    }
}