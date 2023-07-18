using System.Drawing;

namespace Figures.SpawnRandomizer;

public class SpawnRandomizer
{
    private readonly Point _bottomRightPoint;
    private readonly Random _random;
    private SpawnRandomizer(Point bottomRightPoint)
    {
        _bottomRightPoint = bottomRightPoint;
        _random = new Random();
    }
    
    public Point GetRandomPoint(int width, int height)
    {
        return new Point(_random.Next(0, _bottomRightPoint.X - width * 2), _random.Next(0, _bottomRightPoint.Y - height * 2));
    }
    
    public static SpawnRandomizer CreateBasedOnBottomRightPoint(Point bottomRightPoint)
    {
        return new SpawnRandomizer(bottomRightPoint);
    }
}