using H.Core.Enumerations.LocationEnumerationsRegions;

namespace H.Core.Services.Region
{
    public static class RegionSelectionService
    {
        public static RegionNames SelectedRegion { get; set; } = Enum.GetValues(typeof(RegionNames)).Cast<RegionNames>().First();
    }
}