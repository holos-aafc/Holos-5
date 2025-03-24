using System;
using System.Collections.ObjectModel;
using H.Avalonia.Views.ComponentViews;
using H.Core.Enumerations;
using H.Core.Services.StorageService;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace H.Avalonia.ViewModels.SupportingViews.MeasurementUnits
{
    public class UnitsOfMeasurementSelectionViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<MeasurementSystemType> _measurementSystemTypes;
        private MeasurementSystemType _selectedMeasurementType;

        #endregion

        #region Constructors

        public UnitsOfMeasurementSelectionViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, 
            IStorageService storageService) : base(regionManager, eventAggregator, storageService)
        {
            _measurementSystemTypes = new ObservableCollection<MeasurementSystemType>() { MeasurementSystemType.None, MeasurementSystemType.Metric, MeasurementSystemType.Imperial };
            _selectedMeasurementType = MeasurementSystemType.None;
            NextCommand = new DelegateCommand(OnNextExecute, NextCanExecute);
        }

        #endregion

        #region Properties

        public ObservableCollection<MeasurementSystemType> MeasurementSystemTypes
        {
            get { return _measurementSystemTypes; }
        }

        public MeasurementSystemType SelectedMeasurementSystem
        {
            get { return _selectedMeasurementType; }
            set
            {
                if(SetProperty(ref _selectedMeasurementType, value))
                {
                    if (value == MeasurementSystemType.Metric || value == MeasurementSystemType.Imperial)
                    {
                        var activeFarm = StorageService.GetActiveFarm();
                        activeFarm.MeasurementSystemType = value;
                        activeFarm.MeasurementSystemSelected = true;
                    }
                    NextCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public DelegateCommand NextCommand { get; }

        #endregion

        #region Event Handlers

        private void OnNextExecute()
        {
            RegionManager.RequestNavigate(UiRegions.SidebarRegion, nameof(MyComponentsView));
            RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(ChooseComponentsView));
        }

        private bool NextCanExecute()
        {
            return SelectedMeasurementSystem == MeasurementSystemType.Metric ||
                   SelectedMeasurementSystem == MeasurementSystemType.Imperial;
        }

        #endregion
    }
}
