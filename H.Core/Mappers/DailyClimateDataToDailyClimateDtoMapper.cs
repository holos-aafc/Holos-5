using H.Core.Models.Climate;
using H.Core.Providers.Climate;

namespace H.Core.Mappers;

public class DailyClimateDataToDailyClimateDtoMapper : IModelMapper<DailyClimateData, DailyClimateDto>
{
    public DailyClimateDto Map(DailyClimateData source)
    {
        var dest = PropertyMapper.Map<DailyClimateData, DailyClimateDto>(source);
        dest.MeanDailyEvapotranspiration = source.MeanDailyPET;
        return dest;
    }

    /// <summary>In-place transfer path: bridge the differently-named PET property.</summary>
    public void Map(DailyClimateData source, DailyClimateDto dest)
    {
        PropertyMapper.CopyTo(source, dest);
        dest.MeanDailyEvapotranspiration = source.MeanDailyPET;
    }
}
