using Splat;
using System.Configuration;
using H.Core.Enumerations;

namespace H.Core.Helpers;

public class ConfigurationFileHelper
{
    #region Fields

    #endregion

    #region Public Methods

    public static bool BooleanAppSettingIsEnabled(string settingName)
    {
        var result = false;

        try
        {
            ConfigurationManager.RefreshSection("appSettings");

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings.AllKeys.Contains(settingName))
            {
                var setting = config.AppSettings.Settings[settingName].Value;
                result = setting.Equals("true", StringComparison.InvariantCultureIgnoreCase) || setting.Equals("t", StringComparison.InvariantCultureIgnoreCase) || setting.Equals("1", StringComparison.InvariantCultureIgnoreCase);
            }
        }
        catch (Exception e)
        {
        }

        return result;
    }

    public static CountryVersion GetCountryVersion()
    {
        CountryVersion countryVersion = CountryVersion.Canada;
        const string settingName = "CountryVersion";

        try
        {
            ConfigurationManager.RefreshSection("appSettings");

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings.AllKeys.Contains(settingName))
            {
                var setting = config.AppSettings.Settings[settingName].Value;
                if (setting.Equals("ireland", StringComparison.InvariantCultureIgnoreCase))
                {
                    countryVersion = CountryVersion.Ireland;
                }
            }
        }
        catch (Exception e)
        {
        }

        return countryVersion;
    }

    #endregion
}