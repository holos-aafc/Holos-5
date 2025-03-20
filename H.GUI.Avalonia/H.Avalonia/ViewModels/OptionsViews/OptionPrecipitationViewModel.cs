using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
using Prism.Regions;
using H.Core.Services.StorageService;
using H.Core.Enumerations;
using BruTile.Wmts.Generated;
using DynamicData.Kernel;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using System.ComponentModel;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class OptionPrecipitationViewModel : ViewModelBase
    {
        #region Fields
        private PrecipitationDisplayViewModel _data;
        #endregion
        #region Constructors
        public OptionPrecipitationViewModel() { }
        public OptionPrecipitationViewModel(IRegionManager regionManager, IStorageService storageService) : base(regionManager, storageService)
        {
            Data = new PrecipitationDisplayViewModel(storageService);
            GetData();
        }

        #endregion
        #region Properties 
        public PrecipitationDisplayViewModel Data
        {
            get => _data;
            set
            {
                if (_data != value)
                {
                    SetProperty(ref _data, value);
                }
            }
        }

        //public ISeries[] Series { get; set; } =
        //{
        //    new ColumnSeries<double>
        //    {
        //        Values = new double[] {},
        //        Fill = new SolidColorPaint(SKColors.DarkSeaGreen),
        //    }
        //};

        //public Axis[] XAxes { get; set; }
        //    = new Axis[]
        //    {
        //        new Axis
        //        {
        //            Name = "Months",
        //            NamePaint = new SolidColorPaint(SKColors.Black),

        //            LabelsPaint = new SolidColorPaint(SKColors.Black),
        //            TextSize = 14,
        //            LabelsRotation = 20,
        //            ShowSeparatorLines = true,

        //            Labels = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"}
        //        }
        //};
        #endregion
        #region Methods
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
           
        }
        public void GetData()
        {
            ActiveFarm = StorageService.GetActiveFarm();
            Data.January = ActiveFarm.ClimateData.PrecipitationData.January;
            Data.February = ActiveFarm.ClimateData.PrecipitationData.February;
            Data.March = ActiveFarm.ClimateData.PrecipitationData.March;
            Data.April = ActiveFarm.ClimateData.PrecipitationData.April;
            Data.May = ActiveFarm.ClimateData.PrecipitationData.May;
            Data.June = ActiveFarm.ClimateData.PrecipitationData.June;
            Data.July = ActiveFarm.ClimateData.PrecipitationData.July;
            Data.August = ActiveFarm.ClimateData.PrecipitationData.August;
            Data.September = ActiveFarm.ClimateData.PrecipitationData.September;
            Data.October = ActiveFarm.ClimateData.PrecipitationData.October;
            Data.November = ActiveFarm.ClimateData.PrecipitationData.November;
            Data.December = ActiveFarm.ClimateData.PrecipitationData.December;
            //Series[0].Values = Data.PrecipitationSeriesValues;
        }
        #endregion

    }
}
