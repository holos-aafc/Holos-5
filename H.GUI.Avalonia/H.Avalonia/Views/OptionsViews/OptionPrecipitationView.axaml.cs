using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using H.Avalonia.ViewModels.OptionsViews;
using H.Avalonia.ViewModels.SupportingViews.Disclaimer;
using H.Core.Services.StorageService;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SkiaSharp;
using System;

namespace H.Avalonia;

public partial class OptionPrecipitationView : UserControl
{
    private OptionPrecipitationViewModel _viewModel;
    public OptionPrecipitationViewModel? ViewModel => DataContext as OptionPrecipitationViewModel;
    public OptionPrecipitationView(OptionPrecipitationViewModel viewModel)
    {
        InitializeComponent();
        this._viewModel = viewModel;
        this._viewModel.Data.PropertyChanged += OnDataPropertyChanged;
        BuildChart();
    }

    private void OnDataPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {

    }
    private void BuildChart()
    {
        PrecipitationChart.Series = Series;
        PrecipitationChart.XAxes = XAxes;
        Series[0].Values = this._viewModel.Data.PrecipitationSeriesValues;
    }
    public ISeries[] Series { get; set; } =
    {
        new ColumnSeries<double>
        {
            Values = new double[] {},
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

}