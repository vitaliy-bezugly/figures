using System.Drawing;
using System.Xml.Serialization;

namespace Figures.Domain;

[Serializable]
[XmlInclude(typeof(Circle))]
[XmlInclude(typeof(Rectangle))]
[XmlInclude(typeof(Triangle))]
public abstract class Figure
{
    public Figure()
    {
        CentralPoint = new Point(0, 0);
        Speed = new Point(0, 0);
    }

    protected Figure(Point centralPoint, int dX, int dY)
    {
        CentralPoint = centralPoint;
        Speed = new Point(dX, dY);
    }

    public Point Speed { get; set; }
    public Point CentralPoint { get; set; }
    public bool Stopped { get; set; }

    public abstract GeometryFigure Draw();

    public virtual void Move(Point endPoint)
    {
        if(Stopped)
            return;
        
        EnsureRebound(endPoint);
        CentralPoint = new Point(CentralPoint.X + Speed.X, CentralPoint.Y + Speed.Y);
    }
    
    protected abstract void EnsureRebound(Point endPoint);
}