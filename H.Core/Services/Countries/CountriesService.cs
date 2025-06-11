using H.Core.Enumerations.LocationEnumerationsCountries;
using H.Core.Enumerations.LocationEnumerationsRegions;
using H.Core.Services.Region;

namespace H.Core.Services.Countries
{
    public class CountriesService : ICountries
    {
        public IEnumerable<object> GetCountries()
        {
            if (RegionSelectionService.SelectedRegion == RegionNames.Europe)
            {
                return Enum.GetValues(typeof(EuropeCountries)).Cast<object>();
            }
            else if (RegionSelectionService.SelectedRegion == RegionNames.NorthAmerica)
            {
                return Enum.GetValues(typeof(NorthAmericaCountries)).Cast<object>();
            }

            return Enumerable.Empty<object>();
        }
    }
}