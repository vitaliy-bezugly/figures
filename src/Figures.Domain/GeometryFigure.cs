using System.Drawing;

namespace Figures.Domain;

public struct GeometryFigure
{
    public FigureType Type { get; set; }
    public ICollection<Point> Points { get; set; }
    public int? Radius { get; set; }
}