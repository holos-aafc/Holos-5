using H.Infrastructure;

namespace H.Core.Models
{
    // In HOLOS-EU we want to divide all the available components into 4 different categories
    // 1. Site Characteristics
    // 2. Land Use And Management (Land Management)
    // 3. Livestock and
    // 4. Infrastructure
    public enum IrishComponentCategory
    {
        [LocalizedDescription("EnumSiteCharacteristics", typeof(Properties.Resources))]
        SiteCharacteristics,

        [LocalizedDescription("EnumLandManagement", typeof(Properties.Resources))]
        LandManagement,

        [LocalizedDescription("EnumLivestock", typeof(Properties.Resources))]
        Livestock,

        [LocalizedDescription("EnumInfrastructure", typeof(Properties.Resources))]
        Infrastructure,
    }
}
