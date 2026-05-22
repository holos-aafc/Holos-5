using H.Core.Enumerations;
using H.Core.Mappers;
using H.Core.Models;
using H.Core.Models.LandManagement.Fields;
using H.Core.Services.Initialization;
using Prism.Ioc;

namespace H.Core.Factories.Crops;

/// <summary>
/// Default <see cref="ICropFactory"/> implementation. Holds three DI-resolved
/// <see cref="IModelMapper{TSource, TDest}"/> instances that handle the actual property
/// copies between CropViewItem ↔ CropDto + DTO ↔ DTO clones, and an
/// <see cref="ICropInitializationService"/> reference that seeds a new crop's full science
/// defaults (yield, biomass coefficients, residue nitrogen, moisture, lignin, tillage,
/// percentage-returns, energy, economics) before it reaches the user.
///
/// <para><b>Three mapper roles:</b></para>
/// <list type="bullet">
///   <item><c>CropViewItemToCropDtoMapper</c> — domain → DTO when opening the editor for an existing crop.</item>
///   <item><c>CropDtoToCropViewItemMapper</c> — DTO → domain on save.</item>
///   <item><c>CropDtoToCropDtoMapper</c> — clone-a-DTO, used when the editor needs to keep an isolated copy (e.g. "Save As" workflows).</item>
/// </list>
/// All three are resolved by named key out of the container in the constructor.
/// </summary>
public class CropFactory : ICropFactory
{
    #region Fields

    private readonly IModelMapper<CropViewItem, CropDto> _cropViewItemToDtoMapper;
    private readonly IModelMapper<CropDto, CropDto> _cropDtoToDtoMapper;
    private readonly IModelMapper<ICropDto, CropViewItem> _cropDtoToViewItemMapper;

    private readonly ICropInitializationService _cropInitializationService;

    #endregion

    #region Constructors

    public CropFactory(ICropInitializationService cropInitializationService, IContainerProvider containerProvider)
    {
        _cropInitializationService = cropInitializationService ?? throw new ArgumentNullException(nameof(cropInitializationService));

        if (containerProvider == null)
        {
            throw new ArgumentNullException(nameof(containerProvider));
        }
        else
        {
            // The three mappers are registered under named keys (multiple IModelMapper<,> share the
            // same generic signature), so they must be resolved by name rather than by type alone.
            _cropViewItemToDtoMapper = containerProvider.Resolve<IModelMapper<CropViewItem, CropDto>>(nameof(CropViewItemToCropDtoMapper));
            _cropDtoToDtoMapper = containerProvider.Resolve<IModelMapper<CropDto, CropDto>>(nameof(CropDtoToCropDtoMapper));
            _cropDtoToViewItemMapper = containerProvider.Resolve<IModelMapper<ICropDto, CropViewItem>>(nameof(CropDtoToCropViewItemMapper));
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Creates a bare <see cref="CropDto"/> with no defaults applied. Use the
    /// <see cref="CreateDto(Farm)"/> overload when the new crop should be seeded with
    /// province/crop defaults for the user to edit.
    /// </summary>
    public CropDto CreateDto()
    {
        return new CropDto();
    }

    /// <summary>
    /// Creates a fully initialized <see cref="CropDto"/> for a brand-new crop on the given farm.
    /// This is what the field editor calls when the user adds a crop: it seeds a default
    /// (Wheat, current year) <see cref="CropViewItem"/>, runs the initialization service to fill in
    /// all the crop's science defaults (yield, biomass, residue nitrogen, moisture, lignin, energy,
    /// economics) via <c>InitializeCrop</c>, then maps the populated item to a DTO for binding.
    /// </summary>
    public CropDto CreateDto(Farm farm)
    {
        // Seed a sensible default crop. CropType must be set first: the initialization service
        // uses it (with the farm's province/soil/tillage) to look up the per-crop default values.
        var cropViewItem = new CropViewItem();

        cropViewItem.CropType = CropType.Wheat;
        cropViewItem.Year = DateTime.Now.Year;
        cropViewItem.Name = $"{cropViewItem.CropType} {cropViewItem.Year}";

        _cropInitializationService.Initialize(cropViewItem, farm);

        // Map the initialized domain item to the DTO the UI binds to.
        var dto = this.CreateCropDto(cropViewItem);

        return dto;
    }

    /// <summary>
    /// Clones a crop DTO via the DTO→DTO mapper, producing an isolated copy that can be edited
    /// without mutating the original (e.g. the per-year preview cells and "Save As" workflows).
    /// Returns a blank <see cref="CropDto"/> if the template is not a <see cref="CropDto"/>.
    /// </summary>
    public IDto CreateDtoFromDtoTemplate(IDto template)
    {
        if (template is CropDto dtoTemplate)
        {
            return _cropDtoToDtoMapper.Map(dtoTemplate);
        }

        return new CropDto();
    }

    /// <summary>
    /// Maps an existing domain <see cref="CropViewItem"/> to a <see cref="CropDto"/> for UI binding
    /// (domain → DTO). Used when opening the editor for a crop that already exists on a field.
    /// </summary>
    public CropDto CreateCropDto(CropViewItem template)
    {
        return _cropViewItemToDtoMapper.Map(template);
    }

    /// <summary>
    /// Maps a <see cref="ICropDto"/> to a new domain <see cref="CropViewItem"/> (DTO → domain).
    /// This is the inverse of <see cref="CreateCropDto"/> and is what turns the user's edits into
    /// the domain object that gets attached to a field and read by the carbon pipeline.
    /// </summary>
    public CropViewItem CreateCropViewItem(ICropDto cropDto)
    {
        return _cropDtoToViewItemMapper.Map(cropDto);
    }

    /// <summary>
    /// Builds a cover-crop DTO for the given year, marked as a secondary crop and defaulted to the
    /// first valid (non-<see cref="CropType.None"/>) cover-crop type with natural termination.
    /// </summary>
    public CropDto CreateCoverCropDto(int year)
    {
        // Pick the first real cover-crop type (skipping the None sentinel) as the default.
        var coverCropTypes = CropTypeExtensions.GetValidCoverCropTypes().ToList();
        var defaultType = coverCropTypes.FirstOrDefault(ct => ct != CropType.None);

        var dto = new CropDto
        {
            IsSecondaryCrop = true,
            Year = year,
            CropType = defaultType,
            CoverCropTerminationType = CoverCropTerminationType.Natural,
        };

        return dto;
    }

    #endregion
}