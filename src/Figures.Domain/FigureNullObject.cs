using System.Drawing;

namespace Figures.Domain;

public class FigureNullObject : Figure
{
    protected override GeometryFigure GetFigure()
    {
        return new GeometryFigure();
    }

    protected override void EnsureRebound(Point endPoint)
    { }

    protected override System.Drawing.Rectangle GetHitBox()
    {
        return new System.Drawing.Rectangle();
    }
}