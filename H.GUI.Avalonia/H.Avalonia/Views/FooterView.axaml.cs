using System;
using Avalonia.Controls;
using H.Avalonia.ViewModels;
using H.Core.Enumerations;
using H.Core.Services;
using SharpKml.Dom;

namespace H.Avalonia.Views
{
    public partial class FooterView : UserControl
    {
        #region Fields
        
        private string _footerImageTitleCa = "CanadaLogo";
        private string _footerImageTitleIE = "DefaultEULogo";

        private FooterViewModel _footerViewModel;

        #endregion

        #region Constructors
        
        public FooterView(FooterViewModel footerViewModel)
        {
            InitializeComponent();

            if (footerViewModel != null)
            {
                _footerViewModel = footerViewModel;
            }
            else
            {
                throw new ArgumentNullException(nameof(footerViewModel));
            }

            InitializeFooterImage();
        }

        #endregion
        
        #region Public Methods
        
        public void InitializeFooterImage()
        {
            if (_footerViewModel.CountrySettings.Version == CountryVersion.Ireland)
            {
                // found = false | footerImage = null
                var found = this.TryGetResource(_footerImageTitleIE, this.ActualThemeVariant, out var footerImage);
                this.DynamicImage.Source = (global::Avalonia.Media.IImage?)footerImage;
            }
            else
            {
                // found = false | footerImage = null
                var found = this.TryGetResource(_footerImageTitleCa, this.ActualThemeVariant, out var footerImage);
                this.DynamicImage.Source = (global::Avalonia.Media.IImage?)footerImage;
            }
        } 

        #endregion
    }
}