using H.Core.Enumerations;
using H.Core.Services.StorageService;

namespace H.Avalonia.ViewModels.ComponentViews.OtherAnimals
{
    public class DeerComponentViewModel : OtherAnimalsViewModelBase
    {
        #region Constructors

        public DeerComponentViewModel(IStorageService storageService) : base(storageService) 
        {
            ViewName = "Deer";
            OtherAnimalType = AnimalType.Deer;
        }

        public DeerComponentViewModel()
        {

        }

        #endregion
    }
}
