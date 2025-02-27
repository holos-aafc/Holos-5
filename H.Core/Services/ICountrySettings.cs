using H.Core.Enumerations;

namespace H.Core.Services;

public interface ICountrySettings
{
    CountryVersion Version { get; set; }
    Languages Language { get; set; }
}