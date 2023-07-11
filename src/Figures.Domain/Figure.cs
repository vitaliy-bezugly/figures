using System.Drawing;

namespace Figures.Domain;

public abstract class Figure
{
    protected Figure(Point point, Point speed)
    {
        Point = point;
        Speed = speed;
    }

    public Point Speed { get; set; }
    public Point Point { get; set; }

    public abstract IEnumerable<Point> Draw(Point endPoint);
    public abstract void Move(int x, int y);
}