using H.Core.Factories.Crops;
using H.Core.Models.LandManagement.Fields;

namespace H.Core.Mappers;

/// <summary>
/// Maps CropViewItem (v4 domain model) to CropDto (v5 DTO).
/// CropViewItem.Yield maps to CropDto.WetYield.
/// </summary>
public class CropViewItemToCropDtoMapper : IModelMapper<CropViewItem, CropDto>
{
    public CropDto Map(CropViewItem source)
    {
        var dest = PropertyMapper.Map<CropViewItem, CropDto>(source);
        dest.WetYield = source.Yield;

        // PropertyMapper skips the Guid property by design, but the DTO must share
        // the same Guid as its source CropViewItem so that subsequent lookups via
        // FieldComponentService.GetCropViewItemFromDto / RemoveCropFromSystem (which
        // match by Guid) can find the correct view item when the user edits or removes
        // a crop in the UI. Without this, crop type changes in Step 2 are silently
        // dropped because the lookup returns null.
        dest.Guid = source.Guid;
        return dest;
    }

    /// <summary>
    /// In-place display path (used by <c>TransferService.TransferDomainObjectToDto</c>): bridge the
    /// domain's <c>Yield</c> onto the DTO's <c>WetYield</c> so the bound DTO shows the correct wet
    /// yield. Guid is not copied here — the transfer path manages the DTO's identity separately
    /// (<c>DomainObjectGuid</c>).
    /// </summary>
    public void Map(CropViewItem source, CropDto dest)
    {
        PropertyMapper.CopyTo(source, dest);
        dest.WetYield = source.Yield;
    }
}
