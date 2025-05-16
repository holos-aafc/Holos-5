using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
using H.Avalonia.ViewModels.OptionsViews;
using H.Core.Providers.Temperature;
using H.Core.Enumerations;
using System.Collections.ObjectModel;
using System;
using H.Avalonia.ViewModels.Styles;
using Prism.Regions;

namespace H.Avalonia;

public partial class OptionTemperatureView : UserControl
{
    #region Fields

    private OptionTemperatureViewModel _viewModel;
    private ISeries[] _seriesTest = { new ColumnSeries<double> { } };
    private BarChartStyles _barChartsStyles = new BarChartStyles();

    #endregion

    #region Constructors

    public OptionTemperatureView(OptionTemperatureViewModel viewModel)
    {
        InitializeComponent();
        this.Series[0] = _barChartsStyles.ColumnSeriesStyles;
        this._viewModel = viewModel;
        this._viewModel.BindingTemperatureData.PropertyChanged += ViewModel_PropertyChanged;
        BuildChart();
    }

    #endregion

    #region Properties

    private ISeries[] Series
        {
            get { return _seriesTest; }
            set { _seriesTest = value; }
        }
     
    private Axis[] XAxes { get; set; }
         = new Axis[]
        {
            BarChartStyles.BarAxisStyles,
        };

    #endregion

    #region Private Methods

    private void BuildChart()
    {
        TemperatureChart.Series = Series;
        TemperatureChart.XAxes = XAxes;

        var values = new ObservableCollection<double> { };
        foreach (Months month in Enum.GetValues(typeof(Months)))
        {
            {
                values.Add(Math.Round(this._viewModel.BindingTemperatureData.GetValueByMonth(month), 2));
            };
        }
        Series[0].Values = values;
        XAxes[0].Labels = Enum.GetNames(typeof(Months));
        XAxes[0].Name = H.Core.Properties.Resources.Months;
    }

    #endregion

    #region Event Handler

    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        BuildChart();
    }

    #endregion
}


