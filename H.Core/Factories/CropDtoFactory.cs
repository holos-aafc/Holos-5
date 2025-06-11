using AutoMapper;
using H.Core.Models.LandManagement.Fields;

namespace H.Core.Factories;

public class CropDtoFactory : ICropDtoFactory
{
    #region Fields

    private readonly IMapper _cropDtoMapper;

    #endregion

    #region Constructors

    public CropDtoFactory()
    {
        var cropDtoMapperConfiguration = new MapperConfiguration(configuration => { configuration.CreateMap<CropViewItem, CropDto>(); });

        _cropDtoMapper = cropDtoMapperConfiguration.CreateMapper();
    } 

    #endregion

    #region Public Methods

    public ICropDto Create()
    {
        return new CropDto();
    }

    public ICropDto Create(CropViewItem template)
    {
        var cropDto = new CropDto();

        _cropDtoMapper.Map(template, cropDto);

        return cropDto;
    }

    #endregion
}