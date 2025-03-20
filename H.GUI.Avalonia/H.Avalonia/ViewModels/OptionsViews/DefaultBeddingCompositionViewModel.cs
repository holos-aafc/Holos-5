using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using H.Core.Calculators.UnitsOfMeasurement;
using H.Core.Services.StorageService;
using Prism.Events;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class DefaultBeddingCompositionViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<DefaultBeddingCompositionDataViewModel> _beddingCompositionDataViewModels;
        private IUnitsOfMeasurementCalculator _unitsCalculator;

        #endregion

        #region Constructors

        public DefaultBeddingCompositionViewModel(
            IRegionManager regionManager,
            IEventAggregator eventAggregator,
            IStorageService storageService, IUnitsOfMeasurementCalculator unitsCalculator) : base(regionManager, eventAggregator, storageService) 
        {
            if (unitsCalculator != null)
            {
                _unitsCalculator = unitsCalculator;
            }
            else
            {
                throw (new ArgumentNullException(nameof(unitsCalculator)));
            }

            BeddingCompositionDataViewModels = new ObservableCollection<DefaultBeddingCompositionDataViewModel>();

            foreach (var dataClassInstance in base.ActiveFarm.DefaultsCompositionOfBeddingMaterials)
            {
                var dataClassViewModel = new DefaultBeddingCompositionDataViewModel(dataClassInstance, unitsCalculator);
                dataClassViewModel.SetInitializationFlag(true);
                dataClassViewModel.TotalNitrogenKilogramsDryMatter = dataClassInstance.TotalNitrogenKilogramsDryMatter;
                dataClassViewModel.TotalPhosphorusKilogramsDryMatter = dataClassInstance.TotalPhosphorusKilogramsDryMatter;
                dataClassViewModel.TotalCarbonKilogramsDryMatter = dataClassInstance.TotalCarbonKilogramsDryMatter;
                dataClassViewModel.CarbonToNitrogenRatio = dataClassInstance.CarbonToNitrogenRatio;
                dataClassViewModel.SetInitializationFlag(false);
                BeddingCompositionDataViewModels.Add(dataClassViewModel);
            }

            _unitsCalculator.PropertyChanged -= UnitsOfMeasurementChangeListener;
            _unitsCalculator.PropertyChanged += UnitsOfMeasurementChangeListener;
        }

        #endregion

        #region Properties

        public ObservableCollection<DefaultBeddingCompositionDataViewModel> BeddingCompositionDataViewModels
        {
            get => _beddingCompositionDataViewModels;
            set => SetProperty(ref _beddingCompositionDataViewModels, value);
        }

        #endregion

        #region Private Methods

        private void UnitsOfMeasurementChangeListener(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IUnitsOfMeasurementCalculator.IsMetric))
            {
                foreach (var viewModel in  BeddingCompositionDataViewModels)
                {
                    viewModel.UpdateUnitsOfMeasurementDependentProperties();
                }
            }
        }

        #endregion
    }
}
