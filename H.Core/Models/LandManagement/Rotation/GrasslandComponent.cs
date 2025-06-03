#region Imports

using System;
using H.Core.Enumerations;
using H.Core.Models.LandManagement.Rotation;
using H.Infrastructure;

#endregion

namespace H.Core.Models.LandManagement.Rotation.Grasses

{
    /// <summary>
    /// This is a subtypes of Rotation component, please refer to Rotation component
    /// This class will handle the Grasses type but currently we are using all the Rotation features
    /// Next we have to seperate the logics of grasses and make it independent class 
    /// </summary>
    public class GrassesComponent : RotationComponent
    {
        #region Constructors

        public GrassesComponent()


        {

            this.ComponentNameDisplayString = Properties.Resources.TitleRotationGrassesComponent;
            this.ComponentCategory = ComponentCategory.LandManagement;
            this.IrishComponentCategory = IrishComponentCategory.LandManagement;
            this.ComponentType = ComponentType.Rotation;
            base.IrishComponentType = IrishComponentType.Grasses;
            this.ComponentDescriptionString = Properties.Resources.TooltipRotationGrassesComponent;


 
   
        }

        #endregion
    }
}
