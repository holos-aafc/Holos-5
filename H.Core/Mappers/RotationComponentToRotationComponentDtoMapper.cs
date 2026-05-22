using H.Core.Factories.Rotations;
using H.Core.Models.LandManagement.Rotation;

namespace H.Core.Mappers;

public class RotationComponentToRotationComponentDtoMapper : IModelMapper<RotationComponent, RotationComponentDto>
{
    public RotationComponentDto Map(RotationComponent source)
    {
        var dest = PropertyMapper.Map<RotationComponent, RotationComponentDto>(source);
        dest.FieldArea = source.FieldSystemComponent.FieldArea;
        return dest;
    }

    /// <summary>In-place transfer path: carry the derived FieldArea from the field component.</summary>
    public void Map(RotationComponent source, RotationComponentDto dest)
    {
        PropertyMapper.CopyTo(source, dest);
        dest.FieldArea = source.FieldSystemComponent.FieldArea;
    }
}
