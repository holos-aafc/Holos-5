using System.Collections.ObjectModel;
using System.ComponentModel;

namespace H.Core.Factories;

public interface IFieldComponentDto : INotifyPropertyChanged
{
    public string Name { get; set; }
    ObservableCollection<ICropDto> CropDtos { get; set; }
}