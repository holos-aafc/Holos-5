using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H.Core;
using H.Core.Enumerations;
using H.Core.Services;
using H.Core.Services.Animals;

namespace H.Avalonia.ViewModels.OptionsViews.FileMenuViews
{
    public class FileExportManureViewModel : ViewModelBase
    {

        //Unfinished class for exporting manure data
        #region fields
        private readonly IManureService _manureService;
        private readonly IFarmResultsService _farmResultsService;
        #endregion

        #region Constructor
        public FileExportManureViewModel() 
        {
        }
        public FileExportManureViewModel(IManureService manureService, IFarmResultsService farmResultsService, IStorage storage) 
        {
            if (farmResultsService != null)
            {
                _farmResultsService = farmResultsService;
            }
            else
            {
                throw new ArgumentNullException(nameof(farmResultsService));
            }

            if (manureService != null)
            {
                _manureService = manureService;
            }
            else
            {
                throw new ArgumentNullException(nameof(manureService));
            }

        }
        #endregion

        #region Methods
        public override void InitializeViewModel()
        {
            
        }
        #endregion
    }
}
