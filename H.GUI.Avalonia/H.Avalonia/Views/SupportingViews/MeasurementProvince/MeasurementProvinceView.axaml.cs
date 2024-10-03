using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using H.Avalonia.ViewModels.SupportingViews.MeasurementProvince;
using Prism.Regions;
using H.Core.Services;

namespace H.Avalonia.Views.SupportingViews.MeasurementProvince
{
    public partial class MeasurementProvinceView : UserControl
    {
        public MeasurementProvinceView()
        {
            InitializeComponent();
            var regionManager = (IRegionManager)((App)Application.Current).Container.Resolve(typeof(IRegionManager));
            var countrySettings = (ICountrySettings)((App)Application.Current).Container.Resolve(typeof(ICountrySettings));
            DataContext = new MeasurementProvinceViewModel(regionManager, countrySettings);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}