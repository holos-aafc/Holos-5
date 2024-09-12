using Avalonia.Controls;

namespace H.Avalonia.Views
{
    public partial class FooterView : UserControl
    {
        public FooterView()
        {
            InitializeComponent();
            InitializeFooterImage();

        }
        public void InitializeFooterImage()
        {
            // found = false | footerImage = null
            var found = this.TryGetResource("DefaultEULogo", this.ActualThemeVariant, out var footerImage);
            this.DynamicImage.Source = (global::Avalonia.Media.IImage?)footerImage;
        }
    }
}