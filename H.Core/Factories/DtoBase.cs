using Prism.Mvvm;

namespace H.Core.Factories;

public abstract class DtoBase : BindableBase
{
    protected string _name;

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }
}