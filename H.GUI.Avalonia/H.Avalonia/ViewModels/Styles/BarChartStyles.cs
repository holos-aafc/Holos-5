using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace H.Avalonia.ViewModels.Styles
{
    public class BarChartStyles
    {
        //Sets the styles of the bars
        public static readonly ColumnSeries<double> ColumnSeriesStyles = new ColumnSeries<double>
        {
            Fill = new LinearGradientPaint(
            new SKColor(155, 212, 106), new SKColor(83, 123, 58),
                new SKPoint(0.5f, 0),
                new SKPoint(0.5f, 1)),
        };

        //Sets the styles of the axis
        public static readonly Axis BarAxisStyles = new Axis
        {
            NamePaint = new SolidColorPaint(SKColors.Black),
            LabelsPaint = new SolidColorPaint(SKColors.Black),
            TextSize = 14,
            LabelsRotation = 20,
            ShowSeparatorLines = true,
            Padding = new LiveChartsCore.Drawing.Padding(-8, 10, 10, 0),
        };
    }
}
