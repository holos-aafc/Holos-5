using H.Core.Models;
using H.Core.Models.LandManagement.Fields;

namespace H.Core.Services.LandManagement.Fields;

public class FieldInitializationService : IFieldInitializationService
{
    #region Public Methods
    
    public void Initialize(Farm farm, FieldSystemComponent fieldSystemComponent)
    {
        if (fieldSystemComponent.IsInitialized)
        {
            return;
        }

        fieldSystemComponent.Name = this.GetUniqueFieldName(farm.FieldSystemComponents);

        fieldSystemComponent.IsInitialized = true;
    }

    #endregion

    #region Private Methods

    public string GetUniqueFieldName(IEnumerable<FieldSystemComponent> components)
    {
        var i = 1;
        var fieldSystemComponents = components;

        var proposedName = string.Format(Properties.Resources.InterpolatedFieldNumber, i);

        // While proposedName isn't unique, create a unique name for this component so we don't have two or more components with same name
        while (fieldSystemComponents.Any(x => x.Name == proposedName))
        {
            proposedName = string.Format(Properties.Resources.InterpolatedFieldNumber, ++i);
        }

        return proposedName;
    }

    #endregion
}