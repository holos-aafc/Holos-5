using H.Core.Enumerations;

namespace H.Avalonia.ViewModels.ComponentViews.OtherAnimals
{
    public class HorsesComponentViewModel : OtherAnimalsViewModelBase
    {
        #region Constructors
        public HorsesComponentViewModel() 
        {
            ViewName = "Horses";
            OtherAnimalType = AnimalType.Horses;
        }

        #endregion
    }
}