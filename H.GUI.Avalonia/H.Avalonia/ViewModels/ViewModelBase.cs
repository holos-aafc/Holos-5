using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using H.Core.Models;
using H.Core.Services.StorageService;
using Prism.Events;
using System.ComponentModel;
using System.Collections;
using Avalonia;
using Avalonia.Threading;
using System;
using System.Threading.Tasks;

namespace H.Avalonia.ViewModels
{
    public abstract class ViewModelBase : BindableBase, INavigationAware, INotifyDataErrorInfo
    {
        #region Fields

        private Farm _activeFarm;

        protected bool IsInitialized;

        private Storage _storage; 
        private IEventAggregator _eventAggregator;
        private IRegionManager _regionManager;
        private IStorageService _storageService;
        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        #endregion

        #region Constructors

        protected ViewModelBase()
        {
        }

        protected ViewModelBase(Storage storage)
        {
            if (storage != null)
            {
                this.Storage = storage;
            }
            else
            {
                throw new ArgumentNullException(nameof(storage));
            }
        }

        protected ViewModelBase(IStorageService storageService)
        {
            if (storageService != null)
            {
                this.StorageService = storageService;
            }
            else
            {
                throw new ArgumentNullException(nameof(storageService));
            }
        }

        protected ViewModelBase(IEventAggregator eventAggregator)
        {
            if (eventAggregator != null)
            {
                this.EventAggregator = eventAggregator;
            }
            else
            {
                throw new ArgumentNullException(nameof(eventAggregator));
            }
        }

        protected ViewModelBase(IRegionManager regionManager)
        {
            if (regionManager != null)
            {
                RegionManager = regionManager;
            }
            else
            {
                throw new ArgumentNullException(nameof(regionManager));
            }
        }

        protected ViewModelBase(IRegionManager regionManager, IStorageService storageService)
        {
            if (storageService != null)
            {
                this.StorageService = storageService;
            }
            else
            {
                throw new ArgumentNullException(nameof(storageService));
            }

            if (regionManager != null)
            {
                RegionManager = regionManager;
            }
            else
            {
                throw new ArgumentNullException(nameof(regionManager));
            }
        }

        protected ViewModelBase(IRegionManager regionManager, Storage storage) : this(regionManager)
        {
            if (storage != null)
            {
                Storage = storage;
            }
            else
            {
                throw new ArgumentNullException(nameof(storage));
            }
        }

        protected ViewModelBase(
            IRegionManager regionManager, 
            IEventAggregator eventAggregator,
            IStorageService storageService) : this(regionManager)
        {
            if (storageService != null)
            {
                this.StorageService = storageService;
            }

            if(eventAggregator != null)
            {
                this.EventAggregator = eventAggregator;
            }
            else
            {
                throw new ArgumentNullException(nameof(eventAggregator));
            }

            this.SetActiveFarm(this.StorageService);
        }

        protected ViewModelBase(IRegionManager regionManager, IEventAggregator eventAggregator, Storage storage) : this(regionManager, storage)
        {
            if (eventAggregator != null)
            {
                this.EventAggregator = eventAggregator;
            }
            else
            {
                throw new ArgumentNullException(nameof(eventAggregator));
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// A storage file that contains various data items that are shored between viewmodels are passed around the system. This storage
        /// item is instantiated using Prism and through Dependency Injection, is passed within the system.
        /// </summary>
        public Storage Storage
        {
            get => _storage;
            set => SetProperty(ref _storage, value);
        }

        /// <summary>
        /// The notification manager that handles displaying notifications on the page.
        /// </summary>
        public WindowNotificationManager NotificationManager { get; set; }

        protected IRegionManager RegionManager
        {
            get => _regionManager;
            set => SetProperty(ref _regionManager, value);
        }

        protected IEventAggregator EventAggregator 
        {
            get => _eventAggregator;
            set { SetProperty(ref _eventAggregator, value); } 
        }

        public IStorageService StorageService
        {
            get => _storageService;
            set => SetProperty(ref _storageService, value);
        }
        public bool HasErrors => _errors.Any();

        public Farm ActiveFarm
        {
            get => _activeFarm;
            set => SetProperty(ref _activeFarm, value);
        }

        #endregion

        #region Public Methods

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        /// <summary>Navigation validation checker.</summary>
        /// <remarks>Override for Prism 7.2's IsNavigationTarget.</remarks>
        /// <param name="navigationContext">The navigation context.</param>
        /// <returns><see langword="true"/> if this instance accepts the navigation request; otherwise, <see langword="false"/>.</returns>
        public virtual bool OnNavigatingTo(NavigationContext navigationContext)
        {
            return true;
        }
        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName) || !_errors.ContainsKey(propertyName))
            {
                return null;
            }
            return _errors[propertyName];
        }
        public void AddError(string propertyName, string error)
        {
            if (!_errors.ContainsKey(propertyName))
            {
                _errors[propertyName] = new List<string>();
            }

            if (!_errors[propertyName].Contains(error))
            {
                _errors[propertyName].Add(error);
                OnErrorsChanged(propertyName);
                this.RaisePropertyChanged(nameof(HasErrors));
            }
        }
        public void RemoveError(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
            {
                _errors[propertyName].Clear();
                _errors.Remove(propertyName);
                OnErrorsChanged(propertyName);
                this.RaisePropertyChanged(nameof(HasErrors));
            }
        }

        public void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        IEnumerable INotifyDataErrorInfo.GetErrors(string? propertyName)
        {
            return GetErrors(propertyName);
        }

        public void InvokeOnUiThread(Action action)
        {
            if (Dispatcher.UIThread.CheckAccess())
            {
                action();
            }
            else
            {
                Dispatcher.UIThread.Invoke(action);
            }
        }

        #endregion

        #region Private Methods

        private void SetActiveFarm(IStorageService storageService)
        {
            if (storageService != null)
            {
                this.ActiveFarm = storageService.GetActiveFarm();
            }
        }

        #endregion
    }
}
