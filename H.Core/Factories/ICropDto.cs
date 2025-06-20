using System.Collections.ObjectModel;
using System.ComponentModel;
using H.Core.Enumerations;
using H.Core.Models.LandManagement.Fields;

namespace H.Core.Factories;

/// <summary>
/// A data transfer object used to validate and collection information about a <see cref="CropViewItem"/>
/// </summary>
public interface ICropDto : INotifyPropertyChanged, IDto
{
    /// <summary>
    /// The crop type being grown
    /// </summary>
    CropType CropType { get; set; }

    /// <summary>
    /// The list of valid crop types available for selection
    /// </summary>
    ObservableCollection<CropType> ValidCropTypes { get; set; }

    /// <summary>
    /// The year in which the crop was grown
    /// </summary>
    int Year { get; set; }

    double AmountOfIrrigation { get; set; }
}