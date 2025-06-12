using H.Core.Helpers;
using Prism.Mvvm;

namespace H.Core.Factories;

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