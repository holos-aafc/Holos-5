using System.Collections.Generic;
using System.Collections.ObjectModel;
using H.Core.Models;
using Prism.Mvvm;

namespace H.Avalonia.Models;

public class Farm : BindableBase
{
    #region Fields


    #endregion

    #region Constructors

    public Farm()
    {
        this.Components = new List<ComponentBase>();
    }

    #endregion

    #region Properties

    public List<ComponentBase> Components { get; set; }
    public ComponentBase SelectedComponent { get; set; }

    #endregion
}