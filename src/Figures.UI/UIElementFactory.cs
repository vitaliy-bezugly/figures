using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Figures.Domain;

namespace Figures.UI;

public class UiElementFactory
{
    public UIElement Create(GeometryFigure figure)
    {
        var partiallyTransparentSolidColorBrush = GetBrashBasedOnFigureType(figure.Type);

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
    
    private SolidColorBrush GetBrashBasedOnFigureType(FigureType type)
    {
        return type switch
        {
            FigureType.Circle => new SolidColorBrush(GetCircleColor()),
            FigureType.Rectangle => new SolidColorBrush(GetRectangleColor()),
            FigureType.Triangle => new SolidColorBrush(GetTriangleColor()),
        };
    }
    
    private Color GetCircleColor() => Colors.Brown;
    
    private Color GetRectangleColor() => Colors.Blue;
    
    private Color GetTriangleColor() => Colors.Green;
}