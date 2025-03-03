using H.Avalonia.ViewModels.SupportingViews.Disclaimer;
using H.Avalonia.Views.FarmCreationViews;
using H.Avalonia.Views.SupportingViews.Disclaimer;
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
        Storage _storage;

        #endregion

        #region Constructors

        public StartViewModel()
        {

        }

        public StartViewModel(IRegionManager regionManager,
                              IGeographicDataProvider geographicDataProvider,
                              IEventAggregator eventAggregator,
                              DisclaimerViewModel disclaimerViewModel,
                              Storage storage) : base(regionManager, eventAggregator, storage) 
        {
        if (geographicDataProvider != null)
            {
                _geographicDataProvider = geographicDataProvider;
            }
        else
            {
                throw(new ArgumentNullException(nameof(geographicDataProvider)));
            }
        if (disclaimerViewModel != null)
            {
                _disclaimerViewModel = disclaimerViewModel;
            }
            else
            {
                throw (new ArgumentNullException(nameof(disclaimerViewModel)));
            }
            if (storage != null)
            {
                _storage = storage;
            }
        else
            {
                throw (new ArgumentNullException(nameof(storage)));
            }
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
                        //backgroundWorker.DoWork += this.OnBackgroundWorkerDoWork;
                        //backgroundWorker.RunWorkerCompleted += this.OnBackgrounfWorkerCompleted;
                        //backgroundWorker.RunWorkerAsync();
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

        //public void OnNavigatedTo(NavigationContext navigationContext)
        //{
        //    base.EventAggregator.GetEvent<ViewChangedEvent>().Publish(typeof(StartView));

        //    this.IsBusy = true;
        //}

        //private void Initialize()
        //{
        //    _cropDefaultsViewModel.InitializeViewModel();
        //}

        //public bool IsNavigationTarget(NavigationContext navigationContext)
        //{
        //    return true;
        //}

        //public void OnNavigatedFrom(NavigationContext navigationContext)
        //{
        //    this.Initialize();
        //}

        #endregion

        #region Private Methods

        private void OnBackgroundWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //if (sender is BackgroundWorker backgroundWorker)
            //{
            //    backgroundWorker.DoWork -= this.OnBackgroundWorkerDoWork;
            //    backgroundWorker.RunWorkerCompleted -= this.OnBackgroundWorkerCompleted;
            //    //this.InvokeOnUIThread(() => { this.IsBusy = false; });
                
            //    if(base.Storage.Farm.)
            //    else
            //    {
            //        base.RegionManager.RequestNavigate("ContentRegion", nameof(FarmOptionsView));
            //    }
            //}

        }

        private void OnBackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            //try
            //{
            //    // Any exceptions here will not be caught by Application.DispatcherUnhandledException...

            //    // This is running on a separate thread than the main UI thread, need to set culture for this thread
            //    if (Settings.Default.DisplayLanguage.Equals(Core.Enumerations.Languages.French.GetDescription(), StringComparison.InvariantCultureIgnoreCase))
            //    {
            //        var culture = H.Infrastructure.InfrastructureConstants.FrenchCultureInfo;
            //        Thread.CurrentThread.CurrentCulture = culture;
            //        Thread.CurrentThread.CurrentUICulture = culture;
            //    }

            //    this.IsBusyMessage = Properties.Resources.MessageLoadingPleaseWait;

            //    this.ProgressValue = 0;
            //    this.ProgressValue = 25;

            //    _geographicDataProvider.Initialize();

            //    this.ProgressValue = 50;

            //    base.InvokeOnUiThread(() => _mapViewModel.LoadMapFrameworkElements());
            //    this.ProgressValue = 75;

            //    this.ProgressValue = 100;
            //}
            //catch (Exception ex)
            //{
            //    // ... but we can handle it in the throwing thread and pass it to the main thread where Application.DispatcherUnhandledException can handle it

            //    System.Windows.Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action<Exception>((exc) =>
            //    {
            //        throw new Exception($"Exception in {nameof(StartViewModel)}.{nameof(StartViewModel.OnBackgroundWorkerDoWork)}. Error: {ex.Message}", exc);
            //    }), ex);
            //}
        }

        #endregion
    }
}