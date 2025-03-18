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

namespace H.Avalonia;

public partial class OptionBarnTemperatureView : UserControl
{
    #region Fields

    private OptionBarnTemperatureViewModel _viewModel;

    public OptionBarnTemperatureView? ViewModel => DataContext as OptionBarnTemperatureView;

    #endregion

    #region Constructors
    public OptionBarnTemperatureView(OptionBarnTemperatureViewModel viewModel)
    {
        InitializeComponent();
        this._viewModel = viewModel;
        this._viewModel.BindingTemperatureData.PropertyChanged += ViewModel_PropertyChanged;
        BuildChart();
    }

    #endregion

    #region Properties

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
    }

    #endregion

    #region Event Handler

    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        BuildChart();
    }

    #endregion
}


