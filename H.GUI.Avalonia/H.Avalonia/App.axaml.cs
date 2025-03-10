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
using H.Avalonia.ViewModels.ComponentViews.Sheep;
using H.Avalonia.ViewModels.ComponentViews.OtherAnimals;
using H.Avalonia.ViewModels.ComponentViews.Infrastructure;
using H.Avalonia.ViewModels.ComponentViews.Swine;
using H.Avalonia.ViewModels.ComponentViews.Poultry;
using H.Avalonia.ViewModels.SupportingViews.MeasurementProvince;
using H.Avalonia.Views.ComponentViews;
using H.Avalonia.Views.SupportingViews.MeasurementProvince;
using H.Avalonia.Views.SupportingViews.RegionSelection;
using H.Avalonia.ViewModels.SupportingViews.RegionSelection;
using H.Avalonia.ViewModels.OptionsViews;

using H.Core.Services;
using H.Core.Services.Provinces;
using H.Avalonia.Views.SupportingViews.CountrySelection;
using H.Avalonia.ViewModels.SupportingViews.CountrySelection;
using H.Core.Services.RegionCountries;
using H.Avalonia.Views.FarmCreationViews;
using H.Core;
using H.Core.Services.StorageService;
using H.Infrastructure;
using KmlHelpers = H.Avalonia.Infrastructure.KmlHelpers;
using System.Text.RegularExpressions;
using System.Threading;
using H.Core.Enumerations;
using H.Avalonia.ViewModels.SupportingViews.Start;

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
                desktop.Exit += OnExit;
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void OnExit(object sender, ControlledApplicationLifetimeExitEventArgs e)
        {
            var storage = Container.Resolve<IStorage>();
            if (storage != null)
            {
                storage.Save();
            }
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
            containerRegistry.RegisterForNavigation<OptionsView, OptionsViewModel>();
            containerRegistry.RegisterForNavigation<SelectOptionView, SelectOptionViewModel>();
            containerRegistry.RegisterForNavigation<OptionFarmView, OptionFarmViewModel>();
            containerRegistry.RegisterForNavigation<OptionUserSettingsView, OptionUserSettingsViewModel>();
            containerRegistry.RegisterForNavigation<OptionSoilView, OptionSoilViewModel>();
            containerRegistry.RegisterForNavigation<DefaultBeddingCompositionView, DefaultBeddingCompositionViewModel>();
            containerRegistry.RegisterForNavigation<DefaultManureCompositionView, DefaultManureCompositionViewModel>();

            // New development work
            containerRegistry.RegisterForNavigation<OptionBarnTemperatureView, OptionBarnTemperatureViewModel>();
            containerRegistry.RegisterForNavigation<DisclaimerView, DisclaimerViewModel>();
            containerRegistry.RegisterForNavigation<RegionSelectionView, RegionSelectionViewModel>();
            containerRegistry.RegisterForNavigation<MeasurementProvinceView, MeasurementProvinceViewModel>();
            containerRegistry.RegisterForNavigation<CountrySelectionView, CountrySelectionViewModel>();
            containerRegistry.RegisterForNavigation<FarmOptionsView,FarmOptionsViewModel>();
            containerRegistry.RegisterForNavigation<FarmCreationView, FarmCreationViewModel>();
            containerRegistry.RegisterForNavigation<FarmOpenExistingView, FarmOpenExistingViewmodel>();
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
            containerRegistry.RegisterForNavigation<AnaerobicDigestionComponentView, AnaerobicDigestionComponentViewModel>();
            containerRegistry.RegisterForNavigation<GrowerToFinishComponentView, GrowerToFinishComponentViewModel>();
            containerRegistry.RegisterForNavigation<FarrowToWeanComponentView, FarrowToWeanComponentViewModel>();
            containerRegistry.RegisterForNavigation<IsoWeanComponentView, IsoWeanComponentViewModel>();
            containerRegistry.RegisterForNavigation<FarrowToFinishComponentView, FarrowToFinishComponentViewModel>();
            containerRegistry.RegisterForNavigation<ChickenPulletsComponentView, ChickenPulletsComponentViewModel>();
            containerRegistry.RegisterForNavigation<ChickenMultiplierBreederComponentView, ChickenMultiplierBreederComponentViewModel>();
            containerRegistry.RegisterForNavigation<ChickenMeatProductionComponentView, ChickenMeatProductionComponentViewModel>(); 
            containerRegistry.RegisterForNavigation<TurkeyMultiplierBreederComponentView, TurkeyMultiplierBreederComponentViewModel>();
            containerRegistry.RegisterForNavigation<TurkeyMeatProductionComponentView, TurkeyMeatProductionComponentViewModel>();
            containerRegistry.RegisterForNavigation<ChickenEggProductionComponentView, ChickenEggProductionComponentViewModel>();
            containerRegistry.RegisterForNavigation<ChickenMultiplierHatcheryComponentView,  ChickenMultiplierHatcheryComponentViewModel>();
            containerRegistry.RegisterForNavigation<StartView, StartViewModel>();

            // Blank Page
            containerRegistry.RegisterForNavigation<BlankView, BlankViewModel>();

            //containerRegistry.RegisterSingleton<ResultsViewModelBase>();

            #region Storage Registrations

            // V5 object
            containerRegistry.RegisterSingleton<Storage>();

            // V4 object
            containerRegistry.RegisterSingleton<IStorage, H.Core.Storage>();

            containerRegistry.RegisterSingleton<IStorageService, DefaultStorageService>();

            #endregion

            // Providers
            containerRegistry.RegisterSingleton<GeographicDataProvider>();
            containerRegistry.RegisterSingleton<ExportHelpers>();
            containerRegistry.RegisterSingleton<ImportHelpers>();
            containerRegistry.RegisterSingleton<KmlHelpers>();

            containerRegistry.RegisterSingleton<ICountrySettings, CountrySettings>();
            containerRegistry.Register<ICountries, CountriesService>();
            containerRegistry.RegisterSingleton<IProvinces, ProvincesService>();
            
            // Dialogs
            containerRegistry.RegisterDialog<DeleteRowDialog, DeleteRowDialogViewModel>();
        }

        protected override AvaloniaObject CreateShell()
        {
            return base.Container.Resolve<MainWindow>();
        }

        /// <summary>Called after Initialize.</summary>
        protected override void OnInitialized()
        {
            this.SetLanguage();

            // Register views to the Region it will appear in. Don't register them in the ViewModel.
            var regionManager = base.Container.Resolve<IRegionManager>();

            regionManager.RegisterViewWithRegion(UiRegions.ToolbarRegion, typeof(ToolbarView));
                        
            //regionManager.RegisterViewWithRegion(UiRegions.SidebarRegion, typeof(SidebarView));
            regionManager.RegisterViewWithRegion(UiRegions.FooterRegion, typeof(FooterView));
            regionManager.RegisterViewWithRegion(UiRegions.ContentRegion, typeof(DisclaimerView));
            regionManager.RegisterViewWithRegion(UiRegions.ContentRegion, typeof(MeasurementProvinceView));
            regionManager.RegisterViewWithRegion(UiRegions.ContentRegion, typeof(FarmOptionsView));
            regionManager.RegisterViewWithRegion(UiRegions.ContentRegion, typeof(FarmCreationView));
            regionManager.RegisterViewWithRegion(UiRegions.ContentRegion, typeof(FarmOpenExistingView));

            var geographicProvider = Container.Resolve<GeographicDataProvider>();
            geographicProvider.Initialize();
            Container.Resolve<KmlHelpers>();

            var storage = Container.Resolve<IStorage>();
            storage.Load();
        }

        private void SetLanguage()
        {
            var settings = this.Container.Resolve<ICountrySettings>();
            var language = settings.Language;

            if (language == Languages.French)
            {
                H.Avalonia.Resources.Culture = InfrastructureConstants.FrenchCultureInfo;
                H.Core.Properties.Resources.Culture = InfrastructureConstants.FrenchCultureInfo;
            }
        }
    }
}