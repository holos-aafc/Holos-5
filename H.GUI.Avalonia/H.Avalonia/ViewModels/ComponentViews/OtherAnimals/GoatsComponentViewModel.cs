using H.Core.Enumerations;

namespace H.Avalonia.ViewModels.ComponentViews.OtherAnimals
{
    public class GoatsComponentViewModel : OtherAnimalsViewModelBase
    {
        #region Constructors

        public GoatsComponentViewModel() 
        {
            ViewName = "Goats";
            OtherAnimalType = AnimalType.Goats;
        }

        #endregion
    }
}
