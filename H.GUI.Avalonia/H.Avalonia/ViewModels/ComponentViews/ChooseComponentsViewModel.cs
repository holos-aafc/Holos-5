using H.Avalonia.ViewModels;
using H.Avalonia.Views;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace H.Avalonia.ViewModels.ComponentViews
{
    public class ChooseComponentsViewModel : ViewModelBase
    {
        #region Fields

        private string _selectedAvailableComponent;
        private string _selectedComponentTitle;
        private string _selectedComponentDescription;

        private ObservableCollection<string> _availableComponents;

        #endregion

        #region Constructors

        public ChooseComponentsViewModel() { }

        public ChooseComponentsViewModel(IEventAggregator eventAggregator, IRegionManager regionManager, Storage storage) : base(regionManager, eventAggregator, storage)
        {
            this.AvailableComponents = new ObservableCollection<string>() { "Field", "Rotation", "Shelterbelt" };
            this.SelectedAvailableComponent = this.AvailableComponents.ElementAt(1);

            this.PropertyChanged += OnPropertyChanged;
        }

        #endregion

        #region Properties

        public ObservableCollection<string> AvailableComponents
        {
            get => _availableComponents;
            set => SetProperty(ref _availableComponents, value);
        }

        public string SelectedAvailableComponent
        {
            get => _selectedAvailableComponent;
            set => SetProperty(ref _selectedAvailableComponent, value);
        }

        public string SelectedComponentTitle
        {
            get => _selectedComponentTitle;
            set => SetProperty(ref _selectedComponentTitle, value);
        }

        public string SelectedComponentDescription
        {
            get => _selectedComponentDescription;
            set => SetProperty(ref _selectedComponentDescription, value);
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
            this.SelectedAvailableComponent = this.AvailableComponents.First();
        }

        #endregion

        #region Private Methods

        private void UpdateComponentDescription()
        {
        }

        #endregion

        #region Event Handlers

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is nameof(this.SelectedAvailableComponent))
            {
                this.SelectedComponentTitle = this.SelectedAvailableComponent;
                if (this.SelectedAvailableComponent.Equals("Field"))
                {
                    this.SelectedComponentDescription = "A component that allows the user to grow crops";
                }
                else if (this.SelectedAvailableComponent.Equals("Shelterbelt"))
                {
                    this.SelectedComponentDescription = "A component that allows the user to grow trees on the farm";
                }
                else if (this.SelectedAvailableComponent.Equals("Rotation"))
                {
                    this.SelectedComponentDescription = "A component that allows the user create a crop rotation";
                }
            }
        }

        public void OnAddComponentExecute()
        {
            base.Storage.Farm.Components.Add(this.SelectedAvailableComponent);
            base.Storage.Farm.SelectedComponent = this.SelectedAvailableComponent;
        }

        public void OnFinishedAddingComponentsExecute()
        {
            var view = this.RegionManager.Regions[UiRegions.ContentRegion].ActiveViews.Single();
            this.RegionManager.Regions[UiRegions.ContentRegion].Deactivate(view);
            this.RegionManager.Regions[UiRegions.ContentRegion].Remove(view);
        }

        #endregion
    }
}
