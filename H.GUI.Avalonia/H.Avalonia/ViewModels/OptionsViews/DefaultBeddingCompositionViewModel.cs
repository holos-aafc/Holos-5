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

        private ObservableCollection<Table_30_Default_Bedding_Material_Composition_ViewModel> _beddingMaterialCompositionTable30ViewModels;
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

            BeddingMaterialCompositionTable30ViewModels = new ObservableCollection<Table_30_Default_Bedding_Material_Composition_ViewModel>();

            foreach (var dataClassInstance in base.ActiveFarm.DefaultsCompositionOfBeddingMaterials)
            {
                var dataClassViewModel = new Table_30_Default_Bedding_Material_Composition_ViewModel(dataClassInstance, unitsCalculator);
                dataClassViewModel.SetInitializationFlag(true);
                dataClassViewModel.TotalNitrogenKilogramsDryMatter = dataClassInstance.TotalNitrogenKilogramsDryMatter;
                dataClassViewModel.TotalPhosphorusKilogramsDryMatter = dataClassInstance.TotalPhosphorusKilogramsDryMatter;
                dataClassViewModel.TotalCarbonKilogramsDryMatter = dataClassInstance.TotalCarbonKilogramsDryMatter;
                dataClassViewModel.CarbonToNitrogenRatio = dataClassInstance.CarbonToNitrogenRatio;
                dataClassViewModel.SetInitializationFlag(false);
                BeddingMaterialCompositionTable30ViewModels.Add(dataClassViewModel);
            }

            _unitsCalculator.PropertyChanged -= UnitsOfMeasurementChangeListener;
            _unitsCalculator.PropertyChanged += UnitsOfMeasurementChangeListener;
        }

        #endregion

        #region Properties

        public ObservableCollection<Table_30_Default_Bedding_Material_Composition_ViewModel> BeddingMaterialCompositionTable30ViewModels
        {
            get => _beddingMaterialCompositionTable30ViewModels;
            set => SetProperty(ref _beddingMaterialCompositionTable30ViewModels, value);
        }

        #endregion

        #region Private Methods

        private void UnitsOfMeasurementChangeListener(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IUnitsOfMeasurementCalculator.IsMetric))
            {
                foreach (var viewModel in  BeddingMaterialCompositionTable30ViewModels)
                {
                    viewModel.UpdateUnitsOfMeasurementDependentProperties();
                }
            }
        }

        #endregion
    }
}
