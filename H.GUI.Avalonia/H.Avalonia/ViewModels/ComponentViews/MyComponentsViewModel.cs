using System.Collections.ObjectModel;
using System.Linq;
using H.Avalonia.Events;
using H.Avalonia.Views.ComponentViews;
using H.Core.Models;
using Prism.Events;
using Prism.Regions;
using ReactiveUI;

namespace H.Avalonia.ViewModels.ComponentViews;

public class MyComponentsViewModel : ViewModelBase
{
    #region Fields

    private ComponentBase _selectedComponent;
    private ObservableCollection<ComponentBase> _myComponents;

    #endregion

    #region Constructors

    public MyComponentsViewModel()
    {
        this.MyComponents = new ObservableCollection<ComponentBase>();
    }

    public MyComponentsViewModel(Storage storage, IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager, eventAggregator, storage)
    {
        this.MyComponents = new ObservableCollection<ComponentBase>();

        base.EventAggregator.GetEvent<ComponentAddedEvent>().Subscribe(OnComponentAddedEvent);
    }

    #endregion

    #region Properties

    public ComponentBase SelectedComponent
    {
        get => _selectedComponent;
        set => SetProperty(ref _selectedComponent, value);
    }

    public ObservableCollection<ComponentBase> MyComponents
    {
        get => _myComponents;
        set => SetProperty(ref _myComponents, value);
    }

    #endregion

    #region Public Methods

    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        base.OnNavigatedTo(navigationContext);

        this.InitializeViewModel();
    }

    public void InitializeViewModel()
    {
        if (!base.IsInitialized)
        {
            foreach (var component in base.Storage.Farm.Components)
            {
                this.MyComponents.Add(component);
            }

            base.IsInitialized = true;
        }
    }

    #endregion

    #region Event Handlers

    public void OnEditComponentsExecute()
    {
        var activeViews = this.RegionManager.Regions[UiRegions.ContentRegion].ActiveViews;
        if (activeViews != null && activeViews.All(x => x.GetType() != typeof(ChooseComponentsView)))
        {
            this.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(ChooseComponentsView));
        }
    }

    private void OnComponentAddedEvent(ComponentBase componentBase)
    {
        this.MyComponents.Add(componentBase);
        this.SelectedComponent = componentBase;

        base.Storage.Farm.Components.Add(componentBase);
        base.Storage.Farm.SelectedComponent = componentBase;
    }

    #endregion
}