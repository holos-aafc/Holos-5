using H.Core.Properties;
using H.Infrastructure;

namespace H.Core.Enumerations
{
    public enum Province
    {
        [LocalizedDescription("EnumSelectProvince", typeof(Resources))]
        SelectProvince,

        /// <summary>
        /// The names of Ireland's 26 counties 
        /// We are now dividing Ireland into 26 traditional counties
        /// </summary>
        [LocalizedDescription("EnumCarlow", typeof(Resources))]
        Carlow,

        [LocalizedDescription("EnumCavan", typeof(Resources))]
        Cavan,

        [LocalizedDescription("EnumClare", typeof(Resources))]
        Clare,

        [LocalizedDescription("EnumCork", typeof(Resources))]
        Cork,

        [LocalizedDescription("EnumDonegal", typeof(Resources))]
        Donegal,

        [LocalizedDescription("EnumDublin", typeof(Resources))]
        Dublin,

        [LocalizedDescription("EnumGalway", typeof(Resources))]
        Galway,

        [LocalizedDescription("EnumKerry", typeof(Resources))]
        Kerry,

        [LocalizedDescription("EnumKildare", typeof(Resources))]
        Kildare,

        [LocalizedDescription("EnumKilkenny", typeof(Resources))]
        Kilkenny,

        [LocalizedDescription("EnumLaois", typeof(Resources))]
        Laois,

        [LocalizedDescription("EnumLeitrim", typeof(Resources))]
        Leitrim,

        [LocalizedDescription("EnumLimerick", typeof(Resources))]
        Limerick,

        [LocalizedDescription("EnumLongford", typeof(Resources))]
        Longford,

        [LocalizedDescription("EnumLouth", typeof(Resources))]
        Louth,

        [LocalizedDescription("EnumMayo", typeof(Resources))]
        Mayo,

        [LocalizedDescription("EnumMeath", typeof(Resources))]
        Meath,

        [LocalizedDescription("EnumMonaghan", typeof(Resources))]
        Monaghan,

        [LocalizedDescription("EnumOffaly", typeof(Resources))]
        Offaly,

        [LocalizedDescription("EnumRoscommon", typeof(Resources))]
        Roscommon,

        [LocalizedDescription("EnumSligo", typeof(Resources))]
        Sligo,

        [LocalizedDescription("EnumTipperary", typeof(Resources))]
        Tipperary,

        [LocalizedDescription("EnumWaterford", typeof(Resources))]
        Waterford,

        [LocalizedDescription("EnumWestmeath", typeof(Resources))]
        Westmeath,

        [LocalizedDescription("EnumWexford", typeof(Resources))]
        Wexford,

        [LocalizedDescription("EnumWicklow", typeof(Resources))]
        Wicklow,

        [LocalizedDescription("Alberta", typeof(Resources))]
        Alberta,

        [LocalizedDescription("BritishColumbia", typeof(Resources))]
        BritishColumbia,

        [LocalizedDescription("Manitoba", typeof(Resources))]
        Manitoba,

        [LocalizedDescription("NewBrunswick", typeof(Resources))]
        NewBrunswick,

        [LocalizedDescription("Newfoundland", typeof(Resources))]
        Newfoundland,

        [LocalizedDescription("NorthwestTerritories", typeof(Resources))]
        NorthwestTerritories,

        [LocalizedDescription("NovaScotia", typeof(Resources))]
        NovaScotia,

        [LocalizedDescription("Ontario", typeof(Resources))]
        Ontario,

        [LocalizedDescription("Nunavut", typeof(Resources))]
        Nunavut,

        [LocalizedDescription("PrinceEdwardIsland", typeof(Resources))]
        PrinceEdwardIsland,

        [LocalizedDescription("Quebec", typeof(Resources))]
        Quebec,

        [LocalizedDescription("Saskatchewan", typeof(Resources))]
        Saskatchewan,

        [LocalizedDescription("Yukon", typeof(Resources))]
        Yukon,
    }
}