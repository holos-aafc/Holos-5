using H.Core.Models.LandManagement.Fields;

namespace H.Core.Factories;

public interface ICropDtoFactory
{
    #region Public Methods

    ICropDto CreateCropDto();
    ICropDto Create(CropViewItem template);

    #endregion
}