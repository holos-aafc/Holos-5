#region Imports

using System;
using H.Core.Enumerations;
using H.Infrastructure;

#endregion

namespace H.Core.Models.LandManagement.Grassland
{
    public class GrasslandComponent : ComponentBase
    {
        #region Constructors

        public GrasslandComponent()
        {
            this.ComponentNameDisplayString = "Grassland";
            this.ComponentCategory = ComponentCategory.LandManagement;
            this.ComponentType = ComponentType.Grassland;
            this.ComponentDescriptionString = "Represents a grassland component.";
        }

        #endregion

        #region Properties

        // Add grassland-specific properties here

        #endregion
    }
}
