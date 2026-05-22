using H.Core.Factories.Crops;
using H.Core.Models.LandManagement.Fields;

namespace H.Core.Mappers;

/// <summary>
/// Maps ICropDto (v5 DTO) back to CropViewItem (v4 domain model).
/// WetYield on the DTO maps to Yield on CropViewItem.
/// </summary>
public class CropDtoToCropViewItemMapper : IModelMapper<ICropDto, CropViewItem>
{
    public CropViewItem Map(ICropDto source)
    {
        var dest = new CropViewItem();
        PropertyMapper.CopyTo(source, dest);
        dest.Yield = source.WetYield;

        // PropertyMapper skips the Guid property by design, but when a DTO is being
        // added to the model as a new CropViewItem, the view item must share the DTO's
        // Guid so that subsequent lookups via FieldComponentService.GetCropViewItemFromDto
        // / RemoveCropFromSystem (which match by Guid) can find the correct view item
        // when the user later edits or removes that crop.
        dest.Guid = source.Guid;
        return dest;
    }

    /// <summary>
    /// In-place edit path (used by <c>TransferService.TransferDtoToDomainObject</c>): bridge the
    /// DTO's <c>WetYield</c> onto the domain's <c>Yield</c> so yield edits are persisted. Guid is
    /// not copied — the transfer path operates on a cloned DTO with a fresh Guid, and the target
    /// view item already owns the correct Guid.
    /// </summary>
    public void Map(ICropDto source, CropViewItem dest)
    {
        // Cast to the concrete CropDto so PropertyMapper reflects over its full property set, not
        // just the ICropDto interface members (mirrors the create path's Map(ICropDto)).
        PropertyMapper.CopyTo((CropDto)source, dest);
        dest.Yield = source.WetYield;
    }
}
