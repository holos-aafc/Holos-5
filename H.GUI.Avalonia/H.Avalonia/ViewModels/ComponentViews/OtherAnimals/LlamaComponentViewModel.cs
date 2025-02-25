using H.Core.Enumerations;

namespace H.Avalonia.ViewModels.ComponentViews.OtherAnimals
{
    public class LlamaComponentViewModel : OtherAnimalsViewModelBase
    {
        #region

        public LlamaComponentViewModel() 
        {
            ViewName = "Llamas";
            OtherAnimalType = AnimalType.Llamas;
        }

        #endregion
    }
}
