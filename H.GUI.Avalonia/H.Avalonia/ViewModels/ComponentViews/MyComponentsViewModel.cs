using System.Collections.ObjectModel;
using System.Linq;
using H.Avalonia.Events;
using H.Avalonia.Views.ComponentViews;
using Prism.Events;
using Prism.Regions;
using ReactiveUI;

namespace H.Avalonia.ViewModels.ComponentViews;

public class MyComponentsViewModel : ViewModelBase
{
    #region Fields

    private string _selectedItem;
    private ObservableCollection<string> _myComponents;

    #endregion

    #region Constructors

    public MyComponentsViewModel()
    {
        this.MyComponents = new ObservableCollection<string>();
    }

    public MyComponentsViewModel(Storage storage, IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager, eventAggregator, storage)
    {
        this.MyComponents = new ObservableCollection<string>();
        base.EventAggregator.GetEvent<ComponentAddedEvent>().Subscribe(OnComponentAddedEvent);
    }

    public ObservableCollection<string> MyComponents
    {
        get => _myComponents;
        set => SetProperty(ref _myComponents, value);
    }

    #endregion

    #region Properties

    public string SelectedItem
    {
        get => _selectedItem;
        set => SetProperty(ref _selectedItem, value);
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
            foreach (var componentsAsString in base.Storage.Farm.ComponentsAsStrings)
            {
                this.MyComponents.Add(componentsAsString);
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

    private void OnComponentAddedEvent(string component)
    {
        this.MyComponents.Add(component);
        this.SelectedItem = component;

        base.Storage.Farm.ComponentsAsStrings.Add(component);
        base.Storage.Farm.SelectedComponentAsString = component;
    }

    #endregion
}