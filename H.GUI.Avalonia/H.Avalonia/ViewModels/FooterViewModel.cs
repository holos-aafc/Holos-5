using Avalonia.Controls;
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
        public FooterViewModel()
        {
            
            
        }

        public FooterViewModel(IRegionManager regionManager) : base(regionManager) { }
    }
}
