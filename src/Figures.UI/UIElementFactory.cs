using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Figures.Domain;

namespace Figures.UI;

public class UIElementFactory
{
    public UIElement Create(GeometryFigure figure)
    {
        var partiallyTransparentSolidColorBrush = GetRandomBrush();

        if (figure.Type == FigureType.Circle)
        {
            var centralPoint = figure.Points.First();
            return new Ellipse()
            {
                Width = figure.Radius!.Value * 2,
                Height = figure.Radius.Value * 2,
                Fill = partiallyTransparentSolidColorBrush,
                Margin = new Thickness(centralPoint.X - figure.Radius.Value, centralPoint.Y - figure.Radius.Value, 0, 0)
            };
        }
        
        var polygon = new Polygon()
        {
            Fill = partiallyTransparentSolidColorBrush
        };
        
        foreach (var point in figure.Points)
        {
            polygon.Points.Add(new Point(point.X, point.Y));
        }

        return polygon;
    }
    
    private SolidColorBrush GetRandomBrush()
    {
        var random = new Random();
        var color = Colors.Brown;
        return new SolidColorBrush(color)
        {
            Opacity = 0.5
        };
    }
}