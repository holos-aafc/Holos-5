using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace H.Avalonia.Models;

public class Farm : BindableBase
{
    #region Fields

    private string _selectedComponent;

    private ObservableCollection<string> _components;

    #endregion

    #region Constructors

    public Farm()
    {
        this.Components = new ObservableCollection<string>();
    }

    #endregion

    #region Properties

    public ObservableCollection<string> Components
    {
        get => _components;
        set => SetProperty(ref _components, value);
    }

    public string SelectedComponent
    {
        get => _selectedComponent;
        set => SetProperty(ref _selectedComponent, value);
    }

    #endregion
}