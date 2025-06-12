using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace H.Core.Factories;

public class FieldSystemComponentDto : DtoBase, IFieldComponentDto, INotifyDataErrorInfo
{
    #region Fields

    private ObservableCollection<ICropDto> _cropDtoModels;

    #endregion

    #region Constructors

    public FieldSystemComponentDto()
    {
        this.CropDtos = new ObservableCollection<ICropDto>();

        this.PropertyChanged += OnPropertyChanged;
    }

    #endregion

    #region Properties

    public ObservableCollection<ICropDto> CropDtos
    {
        get => _cropDtoModels;
        set => SetProperty(ref _cropDtoModels, value);
    }

    #endregion

    #region Event Handlers

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName.Equals(nameof(Name)))
        {
            var key = nameof(Name);
            if (string.IsNullOrEmpty(_name))
            {
                AddError(key, "Field name cannot be empty");
            }
            else
            {
                RemoveError(key);
            }
        }
    }

    #endregion
}