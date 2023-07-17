using System.Drawing;

namespace Figures.Domain;

[Serializable]
public class Circle : Figure
{
    public Circle() : base()
    {
        Radius = 0;
    }
    
    public Circle(Point centralPoint, int dX, int dY, int radius) : base(centralPoint, dX, dY)
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
                CentralPoint
            },
            Radius = Radius
        };
    }

    protected override void EnsureRebound(Point endPoint)
    {
        if(CentralPoint.X - Radius < 0 || CentralPoint.X + Radius > endPoint.X)
            Speed = new Point(-Speed.X, Speed.Y);
        
        if(CentralPoint.Y - Radius < 0 || CentralPoint.Y + Radius > endPoint.Y)
            Speed = new Point(Speed.X, -Speed.Y);
    }
}