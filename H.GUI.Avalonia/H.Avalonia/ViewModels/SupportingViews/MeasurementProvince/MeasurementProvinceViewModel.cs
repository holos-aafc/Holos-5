using H.Core.Enumerations;
using H.Infrastructure;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace H.Avalonia.ViewModels.SupportingViews.MeasurementProvince
{
    public class MeasurementProvinceViewModel : BindableBase
    {
        #region Fields

        private MeasurementSystemType _selectedMeasurementSystem;
        private Province _selectedProvince;

        #endregion

        #region Constructors

        public MeasurementProvinceViewModel()
        {
            MeasurementSystemCollection = new ObservableCollection<MeasurementSystemType>(EnumHelper.GetValues<MeasurementSystemType>());
            ProvinceCollection = new ObservableCollection<Province>(EnumHelper.GetValues<Province>());
        }

        #endregion

        #region Properties

        public ObservableCollection<MeasurementSystemType> MeasurementSystemCollection { get; set; }
        public ObservableCollection<Province> ProvinceCollection { get; set; }

        public MeasurementSystemType SelectedMeasurementSystem
        {
            get { return _selectedMeasurementSystem; }
            set { SetProperty(ref _selectedMeasurementSystem, value); }
        }

        public Province SelectedProvince
        {
            get { return _selectedProvince; }
            set { SetProperty(ref _selectedProvince, value); }
        }

        #endregion
    }
}