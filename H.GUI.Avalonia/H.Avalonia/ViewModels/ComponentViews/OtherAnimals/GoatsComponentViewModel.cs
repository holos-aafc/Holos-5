using H.Core.Enumerations;
using H.Core.Services.StorageService;

namespace H.Avalonia.ViewModels.ComponentViews.OtherAnimals
{
    public class GoatsComponentViewModel : OtherAnimalsViewModelBase
    {
        #region Constructors

        public GoatsComponentViewModel(IStorageService storageService) : base(storageService) 
        {
            ViewName = "Goats";
            OtherAnimalType = AnimalType.Goats;
        }

        public GoatsComponentViewModel() 
        { 

        }

        #endregion
    }
}
