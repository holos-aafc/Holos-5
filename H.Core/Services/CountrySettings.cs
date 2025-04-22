using H.Core.Enumerations;
using H.Core.Helpers;

namespace H.Core.Services;

public class CountrySettings : ICountrySettings
{
    #region Constructors

    public CountrySettings()
    {
        this.Version = ConfigurationFileHelper.GetCountryVersion();
        this.Language = ConfigurationFileHelper.GetLanguage();
    }

    #endregion

	#region Properties

	public CountryVersion Version { get; set; }

    public Languages Language { get; set; }

    public List<Province> GetProvinces()
    {
        switch (this.Version)
        {
            case CountryVersion.Canada:
            {
                return new List<Province>()
                {
                    Province.Alberta,
                    Province.BritishColumbia,
                    Province.Manitoba,
                    Province.NewBrunswick,
                    Province.Newfoundland,
                    Province.NovaScotia,
                    Province.Ontario,
                    Province.PrinceEdwardIsland,
                    Province.Quebec,
                    Province.Saskatchewan,
                };
            }

            default:
            {
                return new List<Province>();
            }
        }
    }

    #endregion
}