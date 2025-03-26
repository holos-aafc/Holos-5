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
        new ColumnSeries<double>
        {
            Values = new double[] {},
            Fill = new LinearGradientPaint(
                new SKColor(155, 212, 106), new SKColor(83, 123, 58),
                new SKPoint(0.5f, 0),
                new SKPoint(0.5f, 1)),
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
            Padding = new LiveChartsCore.Drawing.Padding(-8, 10, 10, 0),
            Labels = new string[] { H.Core.Properties.Resources.January, H.Core.Properties.Resources.February, H.Core.Properties.Resources.March, H.Core.Properties.Resources.April, H.Core.Properties.Resources.May, H.Core.Properties.Resources.June, H.Core.Properties.Resources.July, H.Core.Properties.Resources.August, H.Core.Properties.Resources.September, H.Core.Properties.Resources.October, H.Core.Properties.Resources.November, H.Core.Properties.Resources.December }
        }
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
    }
    #endregion
    #region Event Handlers
    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        BuildChart();
    }
    #endregion
}