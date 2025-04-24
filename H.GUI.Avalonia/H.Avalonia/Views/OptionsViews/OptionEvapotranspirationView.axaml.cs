using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
using H.Core.Enumerations;
using System.Collections.ObjectModel;
using System;
using H.Avalonia.ViewModels.OptionsViews;
using H.Avalonia.ViewModels.Styles;

namespace H.Avalonia;

public partial class OptionEvapotranspirationView : UserControl
{
    #region Fields
    private OptionEvapotranspirationViewModel _viewModel;
    #endregion
    #region Constructors
    public OptionEvapotranspirationView(OptionEvapotranspirationViewModel viewModel)
    {
        InitializeComponent();
        this._viewModel = viewModel;
        viewModel.BindingEvapotranspirationData.PropertyChanged += ViewModel_PropertyChanged;
        BuildChart();
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
        BarChartStyles.BarAxisStyles
    };
    #endregion
    #region Methods
    private void BuildChart()
    {
        EvotranspirationChart.Series = Series;
        EvotranspirationChart.XAxes = XAxes;

        var values = new ObservableCollection<double> { };
        foreach (Months month in Enum.GetValues(typeof(Months)))
        {
            {
                values.Add(Math.Round(this._viewModel.BindingEvapotranspirationData.GetValueByMonth(month), 2));
            };
        }
        Series[0].Values = values;
        XAxes[0].Labels = Enum.GetNames(typeof(Months));
        XAxes[0].Name = H.Core.Properties.Resources.Months;
    }
    #endregion
    #region Event Handlers
    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        BuildChart();
    }
    #endregion
}