using H.Core.Models.LandManagement.Fields;

namespace H.Core.Factories;

public interface ICropDtoFactory
{
    #region Public Methods

    ICropDto Create();
    ICropDto Create(CropViewItem template);

    #endregion
}