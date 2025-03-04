using H.Core.Enumerations;
using H.Core.Services.StorageService;

namespace H.Avalonia.ViewModels.ComponentViews.OtherAnimals
{
    public class MulesComponentViewModel : OtherAnimalsViewModelBase
    {
        #region Constructors

        public MulesComponentViewModel(IStorageService storageService) : base(storageService) 
        {
            ViewName = "Mules";
            OtherAnimalType = AnimalType.Mules;
        }

        public MulesComponentViewModel() 
        { 
        
        }

        #endregion
    }
}