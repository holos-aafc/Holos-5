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
using Avalonia.Interactivity;
using H.Avalonia.ViewModels.Styles;

namespace H.Avalonia;

public partial class OptionPrecipitationView : UserControl
{
    #region Fields

    private OptionPrecipitationViewModel _viewModel;

    public OptionPrecipitationView? ViewModel => DataContext as OptionPrecipitationView;

    #endregion

    #region Constructors
    public OptionPrecipitationView(OptionPrecipitationViewModel viewModel)
    {
        InitializeComponent();
        this._viewModel = viewModel;
        viewModel.Data.BindingPrecipitationData.PropertyChanged += ViewModel_PropertyChanged;
        //viewModel.Data.PropertyChanged += OnDummyPChanged;
        BuildChart();
    }
    #endregion

    #region Methods
    private void BuildChart()
    {
        PrecipitationChart.Series = Series;
        PrecipitationChart.XAxes = XAxes;

        var values = new ObservableCollection<double> { };
        foreach (Months month in Enum.GetValues(typeof(Months)))
        {
            {
                values.Add(Math.Round(this._viewModel.Data.BindingPrecipitationData.GetValueByMonth(month), 2));
            };
        }
        Series[0].Values = values;
        XAxes[0].Labels = Enum.GetNames(typeof(Months));
        XAxes[0].Name = H.Core.Properties.Resources.Months;
    }
    #endregion

    #region Properties
    public ISeries[] Series { get; set; } =
    {
        BarChartStyles.ColumnSeriesStyles,
    };

    public Axis[] XAxes { get; set; }
        = new Axis[]
    {
        BarChartStyles.BarAxisStyles,
    };
    #endregion

    #region Event Handlers
    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        BuildChart();
    }

    //private void OnDummyPChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    //{
    //    BuildChart();
    //}
    #endregion

}