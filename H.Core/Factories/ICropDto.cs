using System.ComponentModel;
using H.Core.Enumerations;

namespace H.Core.Factories;

public interface ICropDto : INotifyPropertyChanged
{
    CropType CropType { get; set; }
}