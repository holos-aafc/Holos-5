using System.ComponentModel;

namespace H.Core.Factories;

public interface IFieldComponentDto : INotifyPropertyChanged
{
    public string Name { get; set; }
}