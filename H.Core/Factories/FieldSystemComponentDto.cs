using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using H.Core.Models.LandManagement.Fields;

namespace H.Core.Factories;

/// <summary>
/// A class used to validate input as it relates to a <see cref="FieldSystemComponent"/>. This class is used to valid input before any input
/// is transferred to the <see cref="FieldSystemComponent"/>
/// </summary>
public class FieldSystemComponentDto : DtoBase, IFieldComponentDto
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

    /// <summary>
    /// A collection of <see cref="CropDto"/>. Each <see cref="CropDto"/> in the collection represents the crop data input for one particular year on the
    /// given <see cref="FieldSystemComponentDto"/>
    /// </summary>
    public ObservableCollection<ICropDto> CropDtos
    {
        get => _cropDtoModels;
        set => SetProperty(ref _cropDtoModels, value);
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Ensure all <see cref="FieldSystemComponent"/>s will have a valid name specified by the user
    /// </summary>
    private void ValidateFieldName()
    {
        var key = nameof(Name);
        if (string.IsNullOrWhiteSpace(this.Name))
        {
            AddError(key, "Field name cannot be empty");
        }
        else
        {
            RemoveError(key);
        }
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName.Equals(nameof(Name)))
        {
            // Ensure the field name is valid
           ValidateFieldName();
        }
    }

    #endregion
}