using System.Drawing;

namespace Figures.Domain;

public class Triangle : Figure
{
    public Triangle(Point startPoint, int dX, int dY, int edgeLength) : base(startPoint, dX, dY)
    {
        EdgeLength = edgeLength;
    }
    
    public int EdgeLength { get; init; }

    public override GeometryFigure Draw()
    {
        return new GeometryFigure()
        {
            Type = FigureType.Rectangle,
            Points = new List<Point>
            {
                StartPoint,
                new Point(StartPoint.X - (EdgeLength / 2), StartPoint.Y + EdgeLength),
                new Point(StartPoint.X + (EdgeLength / 2), StartPoint.Y + EdgeLength)
            }
        };
    }

    protected override void EnsureRebound(Point endPoint)
    {
        if (StartPoint.X - (EdgeLength / 2) < 0 || StartPoint.X + (EdgeLength / 2) > endPoint.X)
        {
            Speed = new Point(-Speed.X, Speed.Y);
        }

        if (StartPoint.Y < 0 || StartPoint.Y + EdgeLength > endPoint.Y)
        {
            Speed = new Point(Speed.X, -Speed.Y);
        }
    }
}