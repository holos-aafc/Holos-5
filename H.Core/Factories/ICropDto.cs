using System.Collections.ObjectModel;
using System.ComponentModel;
using H.Core.Enumerations;

namespace H.Core.Factories;

public interface ICropDto : INotifyPropertyChanged
{
    CropType CropType { get; set; }
    ObservableCollection<CropType> CropTypes { get; set; }
    int Year { get; set; }
}