using H.Core.Models.LandManagement.Fields;

namespace H.Core.Factories;

public interface ICropDtoFactory
{
    #region Public Methods

    ICropDto CreateCropDto();
    ICropDto CreateCropDto(CropViewItem template);

    #endregion

    ICropDto CreateCropDto(ICropDto template);
}