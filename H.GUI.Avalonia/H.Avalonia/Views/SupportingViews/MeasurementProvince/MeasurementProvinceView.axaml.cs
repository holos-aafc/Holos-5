using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using H.Avalonia.ViewModels.SupportingViews.MeasurementProvince;

namespace H.Avalonia.Views.SupportingViews.MeasurementProvince
{
    public partial class MeasurementProvinceView : UserControl
    {
        public MeasurementProvinceView()
        {
            InitializeComponent();
            DataContext = new MeasurementProvinceViewModel(); 
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}