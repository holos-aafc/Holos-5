using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H.Avalonia.ViewModels.SupportingViews.Start
{
    class StartViewModel : ViewModelBase
    {
        #region Fields

        private bool _isBusy;
        private string _IsBusyMessage;
        private int _progressValue;

        #endregion

        #region Constructors

        public StartViewModel()
        {

        }

        public StartViewModel(IRegionManager regionManager,
                              IEventAggregator eventAggregator,
                              Storage storageService) : base(regionManager, eventAggregator, storageService)
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

    }
}