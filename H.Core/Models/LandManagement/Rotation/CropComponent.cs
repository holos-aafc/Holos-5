#region Imports

using System;
using H.Core.Enumerations;
using H.Infrastructure;

#endregion

namespace H.Core.Models.LandManagement.Crop
{
    public class CropComponent : ComponentBase
    {
        #region Constructors

        public CropComponent()
        {
            this.ComponentNameDisplayString = "Crop";
            this.ComponentCategory = ComponentCategory.LandManagement;
            this.ComponentType = ComponentType.Crop;
            this.ComponentDescriptionString = "Represents a crop component.";
        }

        #endregion

        #region Properties

        // Add crop-specific properties here

        #endregion
    }
}
