using H.Core.Models.LandManagement.Fields;

namespace H.Core.Factories;

public interface ICropFactory
{
    #region Public Methods

    /// <summary>
    /// Create a new instance with no additional configuration to a default instance.
    /// </summary>
    ICropDto CreateCropDto();

    /// <summary>
    /// Create a new instance that is based on the state of an existing <see cref="CropViewItem"/>. This method is used to create a
    /// new instance of a <see cref="CropDto"/> that will be bound to a view.
    /// </summary>
    /// <param name="template">The <see cref="CropViewItem"/> that will be used to provide default values for the new <see cref="CropDto"/> instance</param>
    ICropDto CreateCropDto(CropViewItem template);

    ICropDto CreateCropDto(ICropDto template);

    #endregion

    CropViewItem CreateCropViewItem(ICropDto  cropDto);
}