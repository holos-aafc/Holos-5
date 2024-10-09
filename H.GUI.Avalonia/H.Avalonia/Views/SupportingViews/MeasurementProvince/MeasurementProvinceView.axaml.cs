using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Prism.Regions;
using H.Core.Services;
using H.Avalonia.ViewModels.SupportingViews.MeasurementProvince;

namespace H.Avalonia.Views.SupportingViews.MeasurementProvince
{
    public partial class MeasurementProvinceView : UserControl
    {
        public MeasurementProvinceView(IRegionManager regionManager, ICountrySettings countrySettings)
        {
            InitializeComponent();
            DataContext = new MeasurementProvinceViewModel(regionManager, countrySettings);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}