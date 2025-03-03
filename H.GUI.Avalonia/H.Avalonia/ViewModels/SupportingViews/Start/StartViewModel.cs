using H.Avalonia.ViewModels.SupportingViews.Disclaimer;
using H.Core.Properties;
using H.Core.Providers;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using H.Infrastructure;
using Avalonia;
using Avalonia.Threading;
using H.Avalonia.Views.FarmCreationViews;
using H.Core.Services.StorageService;
using System.Diagnostics;

namespace H.Avalonia.ViewModels.SupportingViews.Start
{
    class StartViewModel : ViewModelBase, INavigationAware
    {
        #region Fields

        private bool _isBusy;
        private string _IsBusyMessage;
        private int _progressValue;

        IGeographicDataProvider _geographicDataProvider;
        DisclaimerViewModel _disclaimerViewModel;
        FarmOptionsViewModel _farmOptionsViewModel;
        FarmCreationViewModel _farmCreationViewModel;
        IStorageService _storageService;

        #endregion

        #region Constructors

        public StartViewModel()
        {

        }
        // IGeographicDataProvider geographicDataProvider,
        public StartViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IStorageService storageService, FarmCreationViewModel farmCreationViewModel, GeographicDataProvider geographicDataProvider) : base(regionManager, eventAggregator, storageService) 
        {

        }

        #endregion

        #region Properties

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                this.SetProperty(ref _isBusy, value, () =>
                {
                    if (_isBusy)
                    {
                        var backgroundWorker = new BackgroundWorker();
                        backgroundWorker.DoWork += this.OnBackgroundWorkerDoWork;
                        backgroundWorker.RunWorkerCompleted += this.OnBackgroundWorkerCompleted;
                        backgroundWorker.RunWorkerAsync();
                    }
                });
            }
        }

        public string IsBusyMessage
        {
            get { return _IsBusyMessage; }
            set { this.SetProperty(ref _IsBusyMessage, value); }
        }

        public int ProgressValue
        {
            get { return _progressValue; }
            set { this.SetProperty(ref _progressValue, value); }
        }

        #endregion

        #region Public Methods

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            this.IsBusy = true;
        }

        private void Initialize()
        {
            //_cropDefaultsViewModel.InitializeViewModel();
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            this.Initialize();
        }

        #endregion

        #region Private Methods

        private void OnBackgroundWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Debug.WriteLine("Top of WorkerCompleted");
            if (sender is BackgroundWorker backgroundWorker)
            {
                backgroundWorker.DoWork -= this.OnBackgroundWorkerDoWork;
                backgroundWorker.RunWorkerCompleted -= this.OnBackgroundWorkerCompleted;
                this.InvokeOnUiThread(() => { this.IsBusy = false; });

                // When first installed, user will have no farms. Show the new farm dialog.
                if (StorageService.GetAllFarms().Count > 0 == false)
                {
                    //_farmCreationViewModel.OnCreateNewFarmExecute();
                }
                else
                {
                    Debug.WriteLine("else block in WorkerCompleted");
                    // If this is not the first run after installation (there is at least one farm), show the landing view.
                    base.RegionManager.RequestNavigate(UiRegions.ContentRegion, nameof(FarmOptionsView));
                    Debug.WriteLine("After ReigonManager Call");
                }
            }
        }

        private void OnBackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Any exceptions here will not be caught by Application.DispatcherUnhandledException...

                // This is running on a separate thread than the main UI thread, need to set culture for this thread
                //if (Settings.Default.DisplayLanguage.Equals(H.Core.Enumerations.Languages.French.GetDescription(), StringComparison.InvariantCultureIgnoreCase))
                //{
                //    var culture = InfrastructureConstants.FrenchCultureInfo;
                //    Thread.CurrentThread.CurrentCulture = culture;
                //    Thread.CurrentThread.CurrentUICulture = culture;
                //}

                this.IsBusyMessage = H.Core.Properties.Resources.MessageLoadingPleaseWait; // make this a resource
                Debug.WriteLine("Before setting progress value");
                this.ProgressValue = 0;
                Debug.WriteLine("After setting 0");
                Thread.Sleep(2000);
                this.ProgressValue = 25;
                Debug.WriteLine("After setting 25");
                Thread.Sleep(2000);
                //_geographicDataProvider.Initialize();

                this.ProgressValue = 50;
                Debug.WriteLine("After setting 50");
                Thread.Sleep(2000);
                //base.InvokeOnUiThread(() => _mapViewModel.LoadMapFrameworkElements());
                this.ProgressValue = 75;
                Debug.WriteLine("After setting 75");
                Thread.Sleep(2000);
                this.ProgressValue = 100;
                Debug.WriteLine("After setting 100");
            }
            catch (Exception ex)
            {
                // ... but we can handle it in the throwing thread and pass it to the main thread where Application.DispatcherUnhandledException can handle it

                Dispatcher.UIThread.Invoke(new Action(() =>
                {
                    throw new Exception($"Exception in {nameof(StartViewModel)}.{nameof(StartViewModel.OnBackgroundWorkerDoWork)}. Error: {ex.Message}", ex);
                }), DispatcherPriority.Normal);
            }
        }

        #endregion
    }
}