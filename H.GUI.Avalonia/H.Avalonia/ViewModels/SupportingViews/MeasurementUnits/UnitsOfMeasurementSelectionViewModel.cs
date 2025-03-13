using System.Collections.ObjectModel;
using H.Core.Enumerations;
using H.Core.Services.StorageService;
using Prism.Events;
using Prism.Regions;

namespace H.Avalonia.ViewModels.SupportingViews.MeasurementUnits
{
    public class UnitsOfMeasurementSelectionViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<MeasurementSystemType> _measurementSystemTypes;

        #endregion

        #region Constructors

        public UnitsOfMeasurementSelectionViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, 
            IStorageService storageService) : base(regionManager, eventAggregator, storageService)
        {
            _measurementSystemTypes = new ObservableCollection<MeasurementSystemType>() { MeasurementSystemType.Metric, MeasurementSystemType.Imperial };
        }

        #endregion

        #region Properties

        public ObservableCollection<MeasurementSystemType> MeasurementSystemTypes
        {
            get { return _measurementSystemTypes; }
        }


        #endregion


    }
}
