using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H.Avalonia.ViewModels.ComponentViews
{
    public class ChooseComponentsViewModelDesign : ChooseComponentsViewModel
    {
        public ChooseComponentsViewModelDesign()
        {
            base.SelectedComponentTitle = "Field Component";
            base.SelectedComponentDescription = "A component that allows the user to grow crops";
        }
    }
}
