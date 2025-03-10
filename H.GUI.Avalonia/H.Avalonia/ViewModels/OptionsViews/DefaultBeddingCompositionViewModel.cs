using System.Collections.ObjectModel;
using H.Core.Services.StorageService;
using Prism.Events;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class DefaultBeddingCompositionViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<Table_30_Default_Bedding_Material_Composition_ViewModel> _beddingMaterialCompositionTable30ViewModels;

        #endregion

        #region Constructors

        public DefaultBeddingCompositionViewModel(
            IRegionManager regionManager,
            IEventAggregator eventAggregator,
            IStorageService storageService) : base(regionManager, eventAggregator, storageService)
        {
            BeddingMaterialCompositionTable30ViewModels = new ObservableCollection<Table_30_Default_Bedding_Material_Composition_ViewModel>();

            foreach (var dataClassInstance in base.ActiveFarm.DefaultsCompositionOfBeddingMaterials)
            {
                var dataClassViewModel = new Table_30_Default_Bedding_Material_Composition_ViewModel(dataClassInstance);
                dataClassViewModel.SetSuppressValidationFlag(true);
                dataClassViewModel.TotalNitrogenKilogramsDryMatter = dataClassInstance.TotalNitrogenKilogramsDryMatter;
                dataClassViewModel.TotalPhosphorusKilogramsDryMatter = dataClassInstance.TotalPhosphorusKilogramsDryMatter;
                dataClassViewModel.TotalCarbonKilogramsDryMatter = dataClassInstance.TotalCarbonKilogramsDryMatter;
                dataClassViewModel.CarbonToNitrogenRatio = dataClassInstance.CarbonToNitrogenRatio;
                dataClassViewModel.SetSuppressValidationFlag(false);
                BeddingMaterialCompositionTable30ViewModels.Add(dataClassViewModel);
            }
        }

        #endregion

        #region Properties

        public ObservableCollection<Table_30_Default_Bedding_Material_Composition_ViewModel> BeddingMaterialCompositionTable30ViewModels
        {
            get => _beddingMaterialCompositionTable30ViewModels;
            set => SetProperty(ref _beddingMaterialCompositionTable30ViewModels, value);
        }

        #endregion
    }
}
