using H.Core.Enumerations;
using H.Core.Models.LandManagement.Fields;
using H.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H.Core.Models.LandManagement.Rotation.Crops
{
    /// <summary>
    /// This is a subtypes of Rotation component, please refer to Rotation component
    /// This class will handle the Crops type but currently we are using all the Rotation features
    /// Next we have to seperate the logics of Crops and make it independent class 
    /// </summary>
    public class CropsComponent : RotationComponent
    {
        #region Constructors

        public CropsComponent()
        {

            this.ComponentNameDisplayString = Properties.Resources.TitleRotationCropsComponent;
            this.ComponentCategory = ComponentCategory.LandManagement;
            this.IrishComponentCategory = IrishComponentCategory.LandManagement;
            this.ComponentType = ComponentType.Rotation;
            base.IrishComponentType = IrishComponentType.Crops;
            this.ComponentDescriptionString = Properties.Resources.TooltipRotationCropsComponent;
        }

        #endregion
    }
}
