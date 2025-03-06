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
                var table30DefaultBeddingViewModel = new Table_30_Default_Bedding_Material_Composition_ViewModel(dataClassInstance);
                BeddingMaterialCompositionTable30ViewModels.Add(table30DefaultBeddingViewModel);
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
