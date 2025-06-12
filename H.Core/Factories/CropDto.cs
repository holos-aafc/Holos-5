using System.Collections.ObjectModel;
using H.Core.Enumerations;
using Prism.Mvvm;

namespace H.Core.Factories;

public class CropDto : DtoBase, ICropDto
{
    #region Fields

    private int _year;
    private CropType _cropType;
    private ObservableCollection<CropType> _cropTypes;

    #endregion

    #region Constructors

    public CropDto()
    {
        this.ValidCropTypes = new ObservableCollection<CropType>() { CropType.NotSelected, CropType.Oats, CropType.Wheat, CropType.Barley };
        this.CropType = this.ValidCropTypes.ElementAt(0);
        this.Year = DateTime.Now.Year;
    }

    #endregion

    #region Properties

    public CropType CropType
    {
        get => _cropType;
        set => SetProperty(ref _cropType, value);
    }

    public ObservableCollection<CropType> ValidCropTypes
    {
        get => _cropTypes;
        set => SetProperty(ref _cropTypes, value);
    }

    public int Year
    {
        get => _year;
        set => SetProperty(ref _year, value);
    }

    #endregion
}