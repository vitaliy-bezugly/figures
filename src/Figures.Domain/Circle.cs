using System.Drawing;

namespace Figures.Domain;

public class Circle : Figure
{
    public Circle(Point startPoint, int dX, int dY, int radius) : base(startPoint, dX, dY)
    {
        Radius = radius;
    }

    public int Radius { get; init; }

    public override GeometryFigure Draw()
    {
        return new GeometryFigure()
        {
            Type = FigureType.Circle,
            Points = new List<Point>
            {
                StartPoint
            },
            Radius = Radius
        };
    }

    protected override void EnsureRebound(Point endPoint)
    {
        if(StartPoint.X - Radius < 0 || StartPoint.X + Radius > endPoint.X)
            Speed = new Point(-Speed.X, Speed.Y);
        
        if(StartPoint.Y - Radius < 0 || StartPoint.Y + Radius > endPoint.Y)
            Speed = new Point(Speed.X, -Speed.Y);
    }
}