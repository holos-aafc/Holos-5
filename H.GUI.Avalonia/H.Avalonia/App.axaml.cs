using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using H.Avalonia.Infrastructure.Dialogs;
using H.Avalonia.ViewModels;
using H.Avalonia.ViewModels.Results;
using H.Avalonia.Views;
using H.Avalonia.Views.ResultViews;
using H.Avalonia.Infrastructure;
using H.Core.Providers;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Regions;
using System;
using ClimateResultsView = H.Avalonia.Views.ResultViews.ClimateResultsView;
using SoilResultsView = H.Avalonia.Views.ResultViews.SoilResultsView;
using H.Avalonia.ViewModels.SupportingViews.Disclaimer;
using H.Avalonia.Views.SupportingViews.Disclaimer;
using H.Avalonia.ViewModels.ComponentViews;
using H.Avalonia.ViewModels.ComponentViews.LandManagement;
using H.Avalonia.ViewModels.ComponentViews.Beef;
using H.Avalonia.ViewModels.ComponentViews.Dairy;
using H.Avalonia.ViewModels.SupportingViews.MeasurementProvince;
using H.Avalonia.Views.ComponentViews;
using H.Avalonia.Views.SupportingViews.MeasurementProvince;
using H.Core.Services;
using H.Core.Services.Provinces;
using H.Avalonia.ViewModels.ComponentViews.Sheep;
using H.Avalonia.ViewModels.ComponentViews.OtherAnimals;

namespace H.Avalonia
{
    public partial class App : PrismApplication
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            base.Initialize();
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }

        /// <summary>Register Services and Views.</summary>
        /// <param name="containerRegistry"></param>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Views - Region Navigation
            containerRegistry.RegisterForNavigation<ToolbarView, ToolbarViewModel>();
            containerRegistry.RegisterForNavigation<SidebarView, SidebarViewModel>();
            containerRegistry.RegisterForNavigation<FooterView, FooterViewModel>();
            containerRegistry.RegisterForNavigation<ClimateDataView, ClimateDataViewModel>();
            containerRegistry.RegisterForNavigation<SoilDataView, SoilDataViewModel>();
            containerRegistry.RegisterForNavigation<AboutPageView, AboutPageViewModel>();
            containerRegistry.RegisterForNavigation<ClimateResultsView, ClimateResultsViewModel>();
            containerRegistry.RegisterForNavigation<SoilResultsView, SoilResultsViewModel>();
            containerRegistry.RegisterForNavigation<MyComponentsView, MyComponentsViewModel>();
            containerRegistry.RegisterForNavigation<ChooseComponentsView, ChooseComponentsViewModel>();
            containerRegistry.RegisterForNavigation<FieldComponentView, FieldComponentViewModel>();

            // New development work
            containerRegistry.RegisterForNavigation<DisclaimerView, DisclaimerViewModel>();
            containerRegistry.RegisterForNavigation<MeasurementProvinceView, MeasurementProvinceViewModel>();
            containerRegistry.RegisterForNavigation<SheepComponentView, SheepComponentViewModel>();
            containerRegistry.RegisterForNavigation<RotationComponentView, RotationComponentViewModel>();
            containerRegistry.RegisterForNavigation<SheepFeedlotComponentView, SheepFeedlotComponentViewModel>();
            containerRegistry.RegisterForNavigation<ShelterbeltComponentView, ShelterbeltComponentViewModel>();
            containerRegistry.RegisterForNavigation<CowCalfComponentView, CowCalfComponentViewModel>();
            containerRegistry.RegisterForNavigation<BackgroundingComponentView, BackgroundingComponentViewModel>();
            containerRegistry.RegisterForNavigation<FinishingComponentView, FinishingComponentViewModel>();
            containerRegistry.RegisterForNavigation<DairyComponentView, DairyComponentViewModel>();
            containerRegistry.RegisterForNavigation<RamsComponentView, RamsComponentViewModel>();
            containerRegistry.RegisterForNavigation<LambsAndEwesComponentView, LambsAndEwesComponentViewModel>();
            containerRegistry.RegisterForNavigation<GoatsComponentView, GoatsComponentViewModel>();
            containerRegistry.RegisterForNavigation<DeerComponentView, DeerComponentViewModel>();
            containerRegistry.RegisterForNavigation<HorsesComponentView, HorsesComponentViewModel>();
            containerRegistry.RegisterForNavigation<MulesComponentView, MulesComponentViewModel>();
            containerRegistry.RegisterForNavigation<BisonComponentView, BisonComponentViewModel>();
            containerRegistry.RegisterForNavigation<LlamaComponentView, LlamaComponentViewModel>();

            // Blank Page
            containerRegistry.RegisterForNavigation<BlankView, BlankViewModel>();

            // 
            //containerRegistry.RegisterSingleton<ResultsViewModelBase>();
            containerRegistry.RegisterSingleton<Storage>();

            // Providers
            containerRegistry.RegisterSingleton<GeographicDataProvider>();
            containerRegistry.RegisterSingleton<ExportHelpers>();
            containerRegistry.RegisterSingleton<ImportHelpers>();
            containerRegistry.RegisterSingleton<KmlHelpers>();

            containerRegistry.RegisterSingleton<ICountrySettings, CountrySettings>();
            containerRegistry.RegisterSingleton<IProvinces, ProvincesService>();


            // Dialogs
            containerRegistry.RegisterDialog<DeleteRowDialog, DeleteRowDialogViewModel>();
        }

        protected override AvaloniaObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        /// <summary>Called after Initialize.</summary>
        protected override void OnInitialized()
        {
            // Register Views to the Region it will appear in. Don't register them in the ViewModel.
            var regionManager = Container.Resolve<IRegionManager>();

            regionManager.RegisterViewWithRegion(UiRegions.ToolbarRegion, typeof(ToolbarView));
                        
            //regionManager.RegisterViewWithRegion(UiRegions.SidebarRegion, typeof(SidebarView));
            regionManager.RegisterViewWithRegion(UiRegions.FooterRegion, typeof(FooterView));
            regionManager.RegisterViewWithRegion(UiRegions.ContentRegion, typeof(DisclaimerView));
            regionManager.RegisterViewWithRegion(UiRegions.ContentRegion, typeof(MeasurementProvinceView));

            var geographicProvider = Container.Resolve<GeographicDataProvider>();
            geographicProvider.Initialize();
            Container.Resolve<KmlHelpers>();
        }
    }
}