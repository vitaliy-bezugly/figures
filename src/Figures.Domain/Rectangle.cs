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
    
    public override IEnumerable<Point> Draw(Point endPoint)
    {
        var points = new List<Point>()
        {
            new Point(StartPoint.X, StartPoint.Y),
            new Point(StartPoint.X + Width, StartPoint.Y),
            new Point(StartPoint.X + Width, StartPoint.Y + Height),
            new Point(StartPoint.X, StartPoint.Y + Height),
        };
        
        return points;
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