using System.Drawing;

namespace Figures.Domain.Args;

public class IntersectionEventArgs : EventArgs
{
    public IntersectionEventArgs(Point intersectionPoint)
    {
        IntersectionPoint = intersectionPoint;
    }

    public Point IntersectionPoint { get; init; }
}