using System.Drawing;

namespace Figures.Domain;

public class Rectangle : Figure
{
    public Rectangle(Point startPoint, int dX, int dY, int width, int height) : base(startPoint, dX, dY)
    {
        Width = width;
        Height = height;
    }

    public int Width { get; set; }
    public int Height { get; set; }
    
    public override GeometryFigure Draw()
    {
        return new GeometryFigure()
        {
            Type = FigureType.Rectangle,
            Points = new List<Point>
            {
                StartPoint,
                new Point(StartPoint.X + Width, StartPoint.Y),
                new Point(StartPoint.X + Width, StartPoint.Y + Height),
                new Point(StartPoint.X, StartPoint.Y + Height)
            },
            Radius = null
        };
    }

    protected override void EnsureRebound(Point endPoint)
    {
        if (StartPoint.X < 0 || StartPoint.X + Width > endPoint.X)
        {
            Speed = new Point(-Speed.X, Speed.Y);
        }

        if (StartPoint.Y < 0 || StartPoint.Y + Height > endPoint.Y)
        {
            Speed = new Point(Speed.X, -Speed.Y);
        }
    }
}