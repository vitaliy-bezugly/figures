using System.Drawing;

namespace Figures.Domain;

[Serializable]
public class Rectangle : Figure
{
    public Rectangle(Point centralPoint, int dX, int dY, int width, int height) : base(centralPoint, dX, dY)
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
                new Point(CentralPoint.X - (Width / 2), CentralPoint.Y - (Height / 2)),
                new Point(CentralPoint.X + (Width / 2), CentralPoint.Y - (Height / 2)),
                new Point(CentralPoint.X + (Width / 2), CentralPoint.Y + (Height / 2)),
                new Point(CentralPoint.X - (Width / 2), CentralPoint.Y + (Height / 2))
            },
            Radius = null
        };
    }

    protected override void EnsureRebound(Point endPoint)
    {
        if(CentralPoint.X - (Width / 2) < 0 || CentralPoint.X + (Width / 2) > endPoint.X)
            Speed = new Point(-Speed.X, Speed.Y);
        
        if(CentralPoint.Y - (Height / 2) < 0 || CentralPoint.Y + (Height / 2) > endPoint.Y)
            Speed = new Point(Speed.X, -Speed.Y);
    }
}