using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using H.Avalonia.Infrastructure;
using H.Avalonia.Infrastructure.Dialogs;
using H.Avalonia.ViewModels;
using H.Avalonia.ViewModels.ComponentViews;
using H.Avalonia.ViewModels.ComponentViews.Beef;
using H.Avalonia.ViewModels.ComponentViews.Dairy;
using H.Avalonia.ViewModels.ComponentViews.Infrastructure;
using H.Avalonia.ViewModels.ComponentViews.LandManagement;
using H.Avalonia.ViewModels.ComponentViews.LandManagement.Field;
using H.Avalonia.ViewModels.ComponentViews.OtherAnimals;
using H.Avalonia.ViewModels.ComponentViews.Poultry;
using H.Avalonia.ViewModels.ComponentViews.Sheep;
using H.Avalonia.ViewModels.ComponentViews.Swine;
using H.Avalonia.ViewModels.FarmCreationViews;
using H.Avalonia.ViewModels.OptionsViews;
using H.Avalonia.ViewModels.OptionsViews.FileMenuViews;
using H.Avalonia.ViewModels.Results;
using H.Avalonia.ViewModels.SupportingViews.CountrySelection;
using H.Avalonia.ViewModels.SupportingViews.Disclaimer;
using H.Avalonia.ViewModels.SupportingViews.MeasurementProvince;
using H.Avalonia.ViewModels.SupportingViews.RegionSelection;
using H.Avalonia.ViewModels.SupportingViews.Start;
using H.Avalonia.Views;
using H.Avalonia.Views.ComponentViews;
using H.Avalonia.Views.FarmCreationViews;
using H.Avalonia.Views.ResultViews;
using H.Avalonia.Views.SupportingViews.CountrySelection;
using H.Avalonia.Views.SupportingViews.Disclaimer;
using H.Avalonia.Views.SupportingViews.MeasurementProvince;
using H.Avalonia.Views.SupportingViews.RegionSelection;
using H.Core;
using H.Core.Calculators.UnitsOfMeasurement;
using H.Core.Enumerations;
using H.Core.Factories;
using H.Core.Providers;
using H.Core.Services;
using H.Core.Services.Countries;
using H.Core.Services.LandManagement.Fields;
using H.Core.Services.Provinces;
using H.Core.Services.StorageService;
using H.Infrastructure;
using Microsoft.Extensions.Logging;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using AutoMapper;
using H.Core.Mappers;
using NLog.Extensions.Logging;
using ClimateResultsView = H.Avalonia.Views.ResultViews.ClimateResultsView;
using KmlHelpers = H.Avalonia.Infrastructure.KmlHelpers;
using SoilResultsView = H.Avalonia.Views.ResultViews.SoilResultsView;

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
            // Logging
            this.SetUpLogging(containerRegistry);

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
            containerRegistry.RegisterForNavigation<OptionFarmView, FarmSettingsViewModel>();
            containerRegistry.RegisterForNavigation<OptionUserSettingsView, UserSettingsViewModel>();
            containerRegistry.RegisterForNavigation<OptionSoilView, SoilSettingsViewModel>();
            containerRegistry.RegisterForNavigation<OptionSoilN2OBreakdownView, SoilN2OBreakdownSettingsViewModel>();
            containerRegistry.RegisterForNavigation<DefaultBeddingCompositionView, DefaultBeddingCompositionViewModel>();
            containerRegistry.RegisterForNavigation<DefaultManureCompositionView, DefaultManureCompositionViewModel>();
            containerRegistry.RegisterForNavigation<OptionPrecipitationView, PrecipitationSettingsViewModel>();


            // New development work
            containerRegistry.RegisterForNavigation<OptionEvapotranspirationView, EvapotranspirationSettingsViewModel>();
            containerRegistry.RegisterForNavigation<OptionTemperatureView, TemperatureSettingsViewModel>();
            containerRegistry.RegisterForNavigation<OptionBarnTemperatureView, BarnTemperatureSettingsViewModel>();
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
            containerRegistry.RegisterForNavigation<FileNewFarmView, FileNewFarmViewModel>();
            containerRegistry.RegisterForNavigation<FileOpenFarmView, FileOpenFarmViewModel>();
            containerRegistry.RegisterForNavigation<FarmManagementView, FarmManagementViewModel>();
            containerRegistry.RegisterForNavigation<FileSaveOptionsView, FileSaveOptionsViewModel>();
            containerRegistry.RegisterForNavigation<FileExportFarmView, FileExportFarmViewModel>();
            containerRegistry.RegisterForNavigation<FileImportFarmView, FileImportFarmViewModel>();
            containerRegistry.RegisterForNavigation<FarmImportFileView, FarmImportFileViewModel>();
            containerRegistry.RegisterForNavigation<FileExportClimateView, FileExportClimateViewModel>();
            containerRegistry.RegisterForNavigation<FileExportManureView, FileExportManureViewModel>();

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

            // Services
            containerRegistry.RegisterSingleton<IFarmHelper, FarmHelper>();
            containerRegistry.RegisterSingleton<IComponentInitializationService, ComponentInitializationService>();
            containerRegistry.RegisterSingleton<IFieldComponentService, FieldComponentService>();
            containerRegistry.RegisterSingleton<IFarmResultsService_NEW, FarmResultsService_NEW>();

            // Unit conversion
            containerRegistry.RegisterSingleton<IUnitsOfMeasurementCalculator, UnitsOfMeasurementCalculator>();
            
            // Dialogs
            containerRegistry.RegisterDialog<DeleteRowDialog, DeleteRowDialogViewModel>();

            // Factories
            containerRegistry.RegisterSingleton<ICropFactory, CropFactory>();
            containerRegistry.RegisterSingleton<IFieldComponentDtoFactory, FieldComponentDtoFactory>();

            // Mappers
            this.SetupMappers(containerRegistry);
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

        private void SetUpLogging(IContainerRegistry containerRegistry)
        {
            // Create a LoggerFactory and add NLog as the logging provider
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.ClearProviders(); // Clear any default providers
                builder.SetMinimumLevel(LogLevel.Trace); // Set your desired minimum log level
                builder.AddNLog(); // Add NLog as the logging provider
            });

            var logger = loggerFactory.CreateLogger<App>();

            // Register the ILogger instance as a singleton in the Prism container
            containerRegistry.RegisterInstance(typeof(ILogger), logger);
        }

        private void SetupMappers(IContainerRegistry containerRegistry)
        {
            var cropDtoToCropDtoConfiguration = new MapperConfiguration(expression =>
            {
                expression.AddProfile<CropDtoCropDtoMapper>();
            });

            var cropDtoToCropVieItemConfiguration = new MapperConfiguration(expression =>
            {
                expression.AddProfile<CropDtoToCropViewItemMapper>();
            });

            var cropViewItemToCropVieItemConfiguration = new MapperConfiguration(expression =>
            {
                expression.AddProfile<CropViewItemToCropDtoMapper>();
            });

            // Register named mappers
            containerRegistry.RegisterInstance<IMapper>(cropDtoToCropDtoConfiguration.CreateMapper(), nameof(CropDtoCropDtoMapper));
            containerRegistry.RegisterInstance<IMapper>(cropDtoToCropVieItemConfiguration.CreateMapper(), nameof(CropDtoToCropViewItemMapper));
            containerRegistry.RegisterInstance<IMapper>(cropViewItemToCropVieItemConfiguration.CreateMapper(), nameof(CropViewItemToCropDtoMapper));
        }
    }
}