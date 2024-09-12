using Avalonia.Controls;
using H.Core.Services;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace H.Avalonia.ViewModels
{
    public class FooterViewModel : ViewModelBase
    {
        #region Fields
        #endregion Fields

        #region Constractors
        public FooterViewModel() { }

        public FooterViewModel(IRegionManager regionManager) : base(regionManager) { }
        #endregion Constractors
    }
}
