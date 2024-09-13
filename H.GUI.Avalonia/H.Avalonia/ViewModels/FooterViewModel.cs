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

        private ICountrySettings _countrySettings;

        #endregion Fields

        #region Constructors

        public FooterViewModel(IRegionManager regionManager, ICountrySettings countrySettings) : base(regionManager)
        {
            if (countrySettings != null)
            {
                _countrySettings = countrySettings;
            }
            else
            {
                throw new ArgumentNullException(nameof(countrySettings));
            }
        }

        #endregion Constructors

        #region Properties

        public ICountrySettings CountrySettings
        {
            get => _countrySettings;
            set => SetProperty(ref _countrySettings, value);
        }

        #endregion
    }
}
