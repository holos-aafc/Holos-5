using H.Core.Enumerations;
using Prism.Mvvm;

namespace H.Core.Factories;

public class CropDto : DtoBase, ICropDto
{
    #region Fields

    private CropType _cropType;

    #endregion

    #region Properties

    public CropType CropType
    {
        get => _cropType;
        set => SetProperty(ref _cropType, value);
    }

    #endregion
}