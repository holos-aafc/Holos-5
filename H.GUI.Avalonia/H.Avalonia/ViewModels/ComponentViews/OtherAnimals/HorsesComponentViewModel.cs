using H.Core.Enumerations;
using H.Core.Services.StorageService;

namespace H.Avalonia.ViewModels.ComponentViews.OtherAnimals
{
    public class HorsesComponentViewModel : OtherAnimalsViewModelBase
    {
        #region Constructors

        public HorsesComponentViewModel(IStorageService storageService) : base(storageService) 
        {
            ViewName = "Horses";
            OtherAnimalType = AnimalType.Horses;
        }

        public HorsesComponentViewModel() 
        { 
        
        }

        #endregion
    }
}