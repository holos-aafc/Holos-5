using System.Collections.ObjectModel;
using H.Core.Models;
using Prism.Mvvm;

namespace H.Avalonia.Models;

public class Farm : BindableBase
{
    #region Fields

    private string _selectedComponentString;

    private ObservableCollection<string> _componentsAsStrings;

    private ComponentBase _selectedComponent;

    #endregion

    #region Constructors

    public Farm()
    {
        this.ComponentsAsStrings = new ObservableCollection<string>();
    }

    #endregion

    #region Properties

    public ObservableCollection<string> ComponentsAsStrings
    {
        get => _componentsAsStrings;
        set => SetProperty(ref _componentsAsStrings, value);
    }

    public string SelectedComponentAsString
    {
        get => _selectedComponentString;
        set => SetProperty(ref _selectedComponentString, value);
    }

    public ComponentBase SelectedComponent
    {
        get => _selectedComponent;
        set =>  _selectedComponent = value;
    }

    #endregion
}