using System.Drawing;
using System.Xml.Serialization;
using Figures.Domain.Args;

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
        Stopped = false;
    }

    protected Figure(Point centralPoint, int dX, int dY)
    {
        CentralPoint = centralPoint;
        Speed = new Point(dX, dY);
        Stopped = false;
    }
    
    public event EventHandler<IntersectionEventArgs>? Intersection;

    public Point Speed { get; set; }
    public Point CentralPoint { get; set; }
    public bool Stopped { get; set; }
    
    public void CheckIntersections(IEnumerable<Figure> figures)
    {
        foreach (var innerFigure in figures)
        {
            if(this == innerFigure)
                continue;

            if (GetType() == innerFigure.GetType())
            {
                IsIntersect(innerFigure);
            }
        }
    }

    public virtual void Move(Point endPoint)
    {
        if(Stopped)
            return;
        
        EnsureRebound(endPoint);
        CentralPoint = new Point(CentralPoint.X + Speed.X, CentralPoint.Y + Speed.Y);
    }
    
    public virtual void IsIntersect(Figure figure)
    {
        System.Drawing.Rectangle currentHitBox = GetHitBox();
        System.Drawing.Rectangle otherHitBox = figure.GetHitBox();
        
        if(currentHitBox.IntersectsWith(otherHitBox))
            OnIntersection(new IntersectionEventArgs(CentralPoint));
    }
    
    public abstract GeometryFigure Draw();
    protected abstract void EnsureRebound(Point endPoint);
    protected abstract System.Drawing.Rectangle GetHitBox();
    
    private void OnIntersection(IntersectionEventArgs e)
    {
        Intersection?.Invoke(this, e);
    }
}