using System.Drawing;

namespace Figures.Domain;

public class Rectangle : Figure
{
    public Rectangle(Point point, Point speed, int width) : base(point, speed)
    {
        Width = width;
    }

    public int Width { get; set; }
    
    public override IEnumerable<Point> Draw(Point endPoint)
    {
        var randomPoint = new Point(Random.Shared.Next(0, (int)endPoint.X), Random.Shared.Next(0, (int)endPoint.Y));

        var points = new List<Point>()
        {
            new Point(randomPoint.X, randomPoint.Y),
            new Point(randomPoint.X + Width, randomPoint.Y),
            new Point(randomPoint.X + Width, randomPoint.Y + Width),
            new Point(randomPoint.X, randomPoint.Y + Width),
        };
        
        return points;
    }

    public override void Move(int x, int y)
    {
        throw new NotImplementedException();
    }
}