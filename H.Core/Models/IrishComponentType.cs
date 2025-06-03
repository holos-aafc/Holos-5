using H.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H.Core.Models
{
    public enum IrishComponentType
    {
        // New Components for HOLOS-IE
        [LocalizedDescription("EnumCrops", typeof(Properties.Resources))]
        Crops,

        [LocalizedDescription("EnumGrasses", typeof(Properties.Resources))]
        Grasses,

        [LocalizedDescription("EnumUnknown", typeof(Properties.Resources))]
        Unknown,

        //[LocalizedDescription("EnumAgroforestry", typeof(Properties.Resources))]
        //Agroforestry,

        //[LocalizedDescription("EnumField", typeof(Properties.Resources))]
        //Field,
    }
}
