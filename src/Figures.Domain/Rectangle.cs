using System.Drawing;

namespace Figures.Domain;

[Serializable]
public class Rectangle : Figure
{
    public Rectangle() : base()
    {
        Width = 0;
        Height = 0;
    }

    public Rectangle(Point centralPoint, int dX, int dY, int width, int height) : base(centralPoint, dX, dY)
    {
        Width = width;
        Height = height;
    }

    public int Width { get; set; }
    public int Height { get; set; }
    
    protected override GeometryFigure GetFigure()
    {
        var hitBox = GetHitBox();
        return new GeometryFigure()
        {
            Type = FigureType.Rectangle,
            Points = new List<Point>
            {
                new(hitBox.X, hitBox.Y),
                new(hitBox.X + hitBox.Width, hitBox.Y),
                new(hitBox.X + hitBox.Width, hitBox.Y + hitBox.Height),
                new Point(hitBox.X, hitBox.Y + hitBox.Height)
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

    protected override System.Drawing.Rectangle GetHitBox()
    {
        return new System.Drawing.Rectangle(CentralPoint.X - (Width / 2), CentralPoint.Y - (Height / 2),
            Width, Height);
    }
}