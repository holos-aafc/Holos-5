using System.Collections.ObjectModel;
using System.Linq;
using H.Avalonia.Views.ComponentViews;
using Prism.Regions;

namespace H.Avalonia.ViewModels.ComponentViews;

public class MyComponentsViewModel : ViewModelBase
{
    #region Fields

    #endregion

    #region Constructors

    public MyComponentsViewModel()
    {
    }

    public MyComponentsViewModel(Storage storage, IRegionManager regionManager) : base(regionManager, storage)
    {
    }

    #endregion

    #region Properties


    #endregion

    #region Public Methods

    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        base.OnNavigatedTo(navigationContext);

        this.InitializeViewModel();
    }

    public void InitializeViewModel()
    {
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

    #endregion
}