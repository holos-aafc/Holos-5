using H.Core.Helpers;
using Prism.Mvvm;

namespace H.Core.Factories;

/// <summary>
/// A base class to be used with any other classes that must validate user input. The properties in this class and subclasses are properties
/// that are bound to GUI controls and should therefore be validated before passes on the input values to the domain/business class objects.
/// </summary>
public abstract class DtoBase : ErrorValidationBase
{
    #region Fields

    protected string _name;

    #endregion

    #region Properties

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    } 

    #endregion
}