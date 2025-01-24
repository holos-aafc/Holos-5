using H.Core.Models;

namespace H.Avalonia.ViewModels.ComponentViews;

public class ComponentTypeToViewTypeMapper
{
    public static string GetViewName(ComponentBase component)
    {
        /*
         * Land Management
         */

        if (component.ComponentType == ComponentType.Field)
        {
            return nameof(FieldComponentView);
        }

        if (component.ComponentType == ComponentType.Rotation)
        {
            return nameof(RotationComponentView);
        }
        if (component.ComponentType == ComponentType.Shelterbelt)
        {
            return nameof(ShelterbeltComponentView);
        }
        
        /*
         * Beef Production
         */

        if (component.ComponentType == ComponentType.CowCalf)
        {
            return nameof(CowCalfComponentView);
        }
        if (component.ComponentType == ComponentType.Backgrounding)
        {
            return nameof(BackgroundingComponentView);
        }
        if (component.ComponentType == ComponentType.Finishing)
        {
            return nameof(FinishingComponentView);
        }


        /*
         * Sheep
         */

        if (component.ComponentType == ComponentType.Sheep)
        {
            return nameof(SheepComponentView);
        }

        if (component.ComponentType == ComponentType.SheepFeedlot)
        {
            return nameof(SheepFeedlotComponentView);
        }

        return string.Empty;
    }
}