using System.Drawing;

namespace Figures.Domain;

[Serializable]
public class Triangle : Figure
{
    public Triangle() : base()
    {
        EdgeLength = 0;
    }
    
    public Triangle(Point centralPoint, int dX, int dY, int edgeLength) : base(centralPoint, dX, dY)
    {
        EdgeLength = edgeLength;
    }
    
    public int EdgeLength { get; init; }

    protected override GeometryFigure GetFigure()
    {
        return new GeometryFigure()
        {
            Type = FigureType.Triangle,
            Points = new List<Point>
            {
                new(CentralPoint.X, CentralPoint.Y - (EdgeLength / 2)),
                new(CentralPoint.X - (EdgeLength / 2), CentralPoint.Y + (EdgeLength / 2)),
                new(CentralPoint.X + (EdgeLength / 2), CentralPoint.Y + (EdgeLength / 2))
            }
        };
    }

    protected override void EnsureRebound(Point endPoint)
    {
        if (CentralPoint.X - (EdgeLength / 2) < 0 || CentralPoint.X + (EdgeLength / 2) > endPoint.X)
        {
            Speed = new Point(-Speed.X, Speed.Y);
        }

        if (CentralPoint.Y - (EdgeLength / 2) < 0 || CentralPoint.Y + (EdgeLength / 2) > endPoint.Y)
        {
            Speed = new Point(Speed.X, -Speed.Y);
        }
    }

    protected override System.Drawing.Rectangle GetHitBox()
    {
        return new System.Drawing.Rectangle(
            new Point(CentralPoint.X - EdgeLength / 2, CentralPoint.Y - EdgeLength / 2), 
            new Size(EdgeLength, EdgeLength));
    }
}