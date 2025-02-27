using H.Core.Enumerations;
using H.Core.Services.StorageService;

namespace H.Avalonia.ViewModels.ComponentViews.OtherAnimals
{
    public class LlamaComponentViewModel : OtherAnimalsViewModelBase
    {
        #region

        public LlamaComponentViewModel(IStorageService storageService) : base(storageService) 
        {
            ViewName = "Llamas";
            OtherAnimalType = AnimalType.Llamas;
        }

        public LlamaComponentViewModel()
        {

        }

        #endregion
    }
}
