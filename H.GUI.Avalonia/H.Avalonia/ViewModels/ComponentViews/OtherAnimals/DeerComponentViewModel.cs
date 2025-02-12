using H.Core.Enumerations;

namespace H.Avalonia.ViewModels.ComponentViews.OtherAnimals
{
    public class DeerComponentViewModel : OtherAnimalsViewModelBase
    {
        #region Constructors

        public DeerComponentViewModel() 
        {
            ViewName = "Deer";
            OtherAnimalType = AnimalType.Deer;
        }

        #endregion
    }
}
