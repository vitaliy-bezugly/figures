using System.Drawing;
using System.Xml.Serialization;
using Figures.Domain.Args;
using Figures.Domain.Exceptions;

namespace Figures.Domain;

[Serializable]
[XmlInclude(typeof(Circle))]
[XmlInclude(typeof(Rectangle))]
[XmlInclude(typeof(Triangle))]
public abstract class Figure
{
    private readonly object _centralPointLock = new();

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
    
    public virtual void MoveToSpecificLocation(Point newLocation)
    {
        lock (_centralPointLock)
        {
            if (Stopped)
                throw new InvalidOperationException("Figure is stopped");

            CentralPoint = newLocation;
            Speed = new Point(Math.Abs(Speed.X), Math.Abs(Speed.Y));
        }
    }

    public virtual void Move(Point endPoint)
    {
        lock (_centralPointLock)
        {
            if(Stopped)
                return;

            if (CheckFigureOutOfRegion(endPoint))
                throw new OutOfRegionException(this, CentralPoint);
        
            EnsureRebound(endPoint);
            CentralPoint = new Point(CentralPoint.X + Speed.X, CentralPoint.Y + Speed.Y);
        }
    }

    public GeometryFigure Draw()
    {
        lock (_centralPointLock)
        {
            return GetFigure();
        }
    }

    protected abstract GeometryFigure GetFigure();
    
    protected virtual bool CheckFigureOutOfRegion(Point endPoint)
    {
        return CentralPoint.X < 0 || CentralPoint.Y < 0  ||
               CentralPoint.X > endPoint.X + 1 || CentralPoint.Y > endPoint.Y + 1;
    }
    
    protected virtual void IsIntersect(Figure figure)
    {
        System.Drawing.Rectangle currentHitBox = GetHitBox();
        System.Drawing.Rectangle otherHitBox = figure.GetHitBox();
        
        if(currentHitBox.IntersectsWith(otherHitBox))
            OnIntersection(new IntersectionEventArgs(CentralPoint));
    }
    
    protected abstract void EnsureRebound(Point endPoint);
    protected abstract System.Drawing.Rectangle GetHitBox();
    
    private void OnIntersection(IntersectionEventArgs e)
    {
        Intersection?.Invoke(this, e);
    }
}