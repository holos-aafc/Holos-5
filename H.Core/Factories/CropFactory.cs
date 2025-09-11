using AutoMapper;
using H.Core.Enumerations;
using H.Core.Mappers;
using H.Core.Models.LandManagement.Fields;
using H.Infrastructure;
using Prism.Ioc;

namespace H.Core.Factories;

/// <summary>
/// A class used to create new <see cref="CropDto"/> and <see cref="CropViewItem"/> instances. The class will provide basic initialization of a new instance before returning the result to the caller.
/// </summary>
public class CropFactory : ICropFactory
{
    #region Fields

    private readonly IMapper _cropViewItemToDtoMapper;
    private readonly IMapper _cropDtoToDtoMapper;
    private readonly IMapper _cropDtoToViewItemMapper;

    #endregion

    #region Constructors

    public CropFactory(IContainerProvider containerProvider)
    {
        _cropViewItemToDtoMapper = containerProvider.Resolve<IMapper>(nameof(CropViewItemToCropDtoMapper));
        _cropDtoToDtoMapper = containerProvider.Resolve<IMapper>(nameof(CropDtoCropDtoMapper));
        _cropDtoToViewItemMapper = containerProvider.Resolve<IMapper>(nameof(CropDtoToCropViewItemMapper));
    } 

    #endregion

    #region Public Methods

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

    public ICropDto CreateCropDto(CropViewItem template)
    {
        var cropDto = new CropDto();

        _cropViewItemToDtoMapper.Map(template, cropDto);

        return cropDto;
    }

    public CropViewItem CreateCropViewItem(ICropDto  cropDto)
    {
        var viewItem = new CropViewItem();

        _cropDtoToViewItemMapper.Map(cropDto, viewItem);

        return viewItem;
    }

    #endregion
}