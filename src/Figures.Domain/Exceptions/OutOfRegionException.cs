using System.Drawing;

namespace Figures.Domain.Exceptions;

public class OutOfRegionException : Exception
{
    public OutOfRegionException(Figure figure, Point currentPoint) : base($"Figure is out of region at point: {currentPoint}")
    {
        Figure = figure;
        CurrentPoint = currentPoint;
    }

    public OutOfRegionException(Figure figure, Point currentPoint, Exception innerException) : base($"Figure is out of region at point: {currentPoint}", innerException)
    {
        Figure = figure;
        CurrentPoint = currentPoint;
    }

    public Point CurrentPoint { get; init; }
    public Figure Figure { get; init; }
}