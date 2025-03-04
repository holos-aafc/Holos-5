using H.Core.Enumerations;
using H.Core.Services.StorageService;

namespace H.Avalonia.ViewModels.ComponentViews.OtherAnimals
{
    public class BisonComponentViewModel : OtherAnimalsViewModelBase
    {
        #region Constructors

        public BisonComponentViewModel(IStorageService storageService) : base(storageService)
        {
            ViewName = "Bison";
            OtherAnimalType = AnimalType.Bison;
        }

        public BisonComponentViewModel() 
        { 
        
        }

        #endregion
    }
}
