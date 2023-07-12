using System.Drawing;

namespace Figures.Domain;

public abstract class Figure
{
    protected Figure(Point startPoint, int dX, int dY)
    {
        StartPoint = startPoint;
        Speed = new Point(dX, dY);
    }

    public Point Speed { get; set; }
    public Point StartPoint { get; set; }

    public abstract GeometryFigure Draw();

    public virtual void Move(Point endPoint)
    {
        EnsureRebound(endPoint);
        StartPoint = new Point(StartPoint.X + Speed.X, StartPoint.Y + Speed.Y);
    }
    
    protected abstract void EnsureRebound(Point endPoint);
}