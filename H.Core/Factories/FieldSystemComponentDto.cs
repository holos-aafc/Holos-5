using System.Collections;
using System.ComponentModel;

namespace H.Core.Factories;

public class FieldSystemComponentDto : DtoBase, IFieldComponentDto, INotifyDataErrorInfo
{
    #region Fields

    #endregion

    #region Constructors

    public FieldSystemComponentDto()
    {
        this.PropertyChanged += OnPropertyChanged;
    }

    #endregion

    #region Properties

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