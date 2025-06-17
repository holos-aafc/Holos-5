using AutoMapper;
using H.Core.Enumerations;
using H.Core.Models.LandManagement.Fields;
using H.Infrastructure;

namespace H.Core.Factories;

/// <summary>
/// A class used to create new <see cref="CropDto"/> instances. The class will provide basic initialization of a new instance before returning the result to the caller.
/// </summary>
public class CropDtoFactory : ICropDtoFactory
{
    #region Fields

    private readonly IMapper _cropViewItemToDtoMapper;
    private readonly IMapper _cropDtoToDtoMapper;

    #endregion

    #region Constructors

    public CropDtoFactory()
    {
        var cropViewItemToDtoMapperConfiguration = new MapperConfiguration(configuration => { configuration.CreateMap<CropViewItem, CropDto>(); });
        var cropDtoToDtoMapperConfiguration = new MapperConfiguration(configuration => { configuration.CreateMap<CropDto, CropDto>(); });

        _cropViewItemToDtoMapper = cropViewItemToDtoMapperConfiguration.CreateMapper();
        _cropDtoToDtoMapper = cropDtoToDtoMapperConfiguration.CreateMapper();
    } 

    #endregion

    #region Public Methods

    /// <summary>
    /// Create a new instance with no additional configuration to a default instance.
    /// </summary>
    public ICropDto CreateCropDto()
    {
        return new CropDto();
    }

    public ICropDto CreateCropDto(ICropDto template)
    {
        var cropDto = new CropDto();

        _cropDtoToDtoMapper.Map(template, cropDto);

        return cropDto;
    }

    /// <summary>
    /// Create a new instance that is based on the state of an existing <see cref="CropViewItem"/>. This method is used to create a
    /// new instance of a <see cref="CropDto"/> that will be bound to a view.
    /// </summary>
    /// <param name="template">The <see cref="CropViewItem"/> that will be used to provide default values for the new <see cref="CropDto"/> instance</param>
    public ICropDto CreateCropDto(CropViewItem template)
    {
        var cropDto = new CropDto();

        _cropViewItemToDtoMapper.Map(template, cropDto);

        return cropDto;
    }

    #endregion
}