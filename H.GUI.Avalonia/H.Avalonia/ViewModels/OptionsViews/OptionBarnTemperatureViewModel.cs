using H.Core.Enumerations;
using H.Core.Providers.AnaerobicDigestion;
using H.Core.Providers.Climate;
using H.Core.Providers.Temperature;
using H.Core.Services.StorageService;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using LiveChartsCore.SkiaSharpView.VisualElements;
using BruTile.Wmts.Generated;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Drawing;
using System.ComponentModel;
using H.Core.Calculators.UnitsOfMeasurement;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class OptionBarnTemperatureViewModel : ViewModelBase
    {
        #region Fields

        private TemperatureData _bindingTemperatureData = new TemperatureData();
        private IIndoorTemperatureProvider _indoorTemperatureProvider = new Table_63_Indoor_Temperature_Provider();
        //private readonly UnitsOfMeasurementCalculator _unitOfMeasurementCalculator = new UnitsOfMeasurementCalculator();

        #endregion

        #region Constructors

        public OptionBarnTemperatureViewModel()
        {

        }

        public OptionBarnTemperatureViewModel(IRegionManager regionManager, IStorageService storageService) : base(regionManager, storageService)
        {
            CreateBarnTemperatureSeries();
        }

        #endregion

        #region Properties

        public TemperatureData BindingTemperatureData
        {
            get => _bindingTemperatureData;
            set => SetProperty(ref _bindingTemperatureData, value);
        }

        public ISeries[] Series { get; set; } =
        {
            new ColumnSeries<double>
            {
                Values = new double[] { 2.77, 2.38, 4.95, 7.79, 12.21, 17.71, 19.00, 17.92, 14.18, 7.39, 4.87, 3.82 },
                Fill = new SolidColorPaint(SKColors.DarkSeaGreen),
            }
        };

        public Axis[] XAxes { get; set; }
            = new Axis[]
            {
                new Axis
                {
                    Name = "Months",
                    NamePaint = new SolidColorPaint(SKColors.Black),

                    LabelsPaint = new SolidColorPaint(SKColors.Black),
                    TextSize = 14,
                    LabelsRotation = 20,
                    ShowSeparatorLines = true,

                    Labels = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"}
                }
            };


        #endregion

        #region Public Methods

        public void CreateBarnTemperatureSeries()
        {
            BindingTemperatureData.January = _indoorTemperatureProvider.GetIndoorTemperatureForMonth(Province.Alberta, Months.January);
            BindingTemperatureData.February = _indoorTemperatureProvider.GetIndoorTemperatureForMonth(Province.Alberta, Months.February);
            BindingTemperatureData.March = _indoorTemperatureProvider.GetIndoorTemperatureForMonth(Province.Alberta, Months.March);
            BindingTemperatureData.April = _indoorTemperatureProvider.GetIndoorTemperatureForMonth(Province.Alberta, Months.April);
            BindingTemperatureData.May = _indoorTemperatureProvider.GetIndoorTemperatureForMonth(Province.Alberta, Months.May);
            BindingTemperatureData.June = _indoorTemperatureProvider.GetIndoorTemperatureForMonth(Province.Alberta, Months.June);
            BindingTemperatureData.July = _indoorTemperatureProvider.GetIndoorTemperatureForMonth(Province.Alberta, Months.July);
            BindingTemperatureData.August = _indoorTemperatureProvider.GetIndoorTemperatureForMonth(Province.Alberta, Months.August);
            BindingTemperatureData.September = _indoorTemperatureProvider.GetIndoorTemperatureForMonth(Province.Alberta, Months.September);
            BindingTemperatureData.October = _indoorTemperatureProvider.GetIndoorTemperatureForMonth(Province.Alberta, Months.October);
            BindingTemperatureData.November = _indoorTemperatureProvider.GetIndoorTemperatureForMonth(Province.Alberta, Months.November);
            BindingTemperatureData.December = _indoorTemperatureProvider.GetIndoorTemperatureForMonth(Province.Alberta, Months.December);

        }

        public void InitializeBindingBarnTemperatureData()
        {
        }




        #endregion

        #region Private Methods



        #endregion

        #region Event Handlers

        private void BindingBarnTemperatureOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

       #endregion
    }
}