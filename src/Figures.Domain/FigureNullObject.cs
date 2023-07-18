using System.Drawing;

namespace Figures.Domain;

public class FigureNullObject : Figure
{
    public override GeometryFigure Draw()
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