using System;
using System.Drawing;
using Figures.Domain;

namespace Figures.UI;

public class FiguresBuilder
{
    public Figure BuildCircle(Point rightBottomPoint)
    {
        var radius = Random.Shared.Next(20, 50);
        
        Point spawnPoint = GetSpawnCoordinate(rightBottomPoint);
        var (dx, dy) = GetDxAndDy();
        
        return new Circle(spawnPoint, dx, dy, radius);
    }

    public Figure BuildRectangle(Point rightBottomPoint)
    {
        var width = Random.Shared.Next(20, 50);
        var height = Random.Shared.Next(20, width);
        
        Point spawnPoint = GetSpawnCoordinate(rightBottomPoint);
        var (dx, dy) = GetDxAndDy();
        
        return new Domain.Rectangle(spawnPoint, dx, dy, width, height);
    }

    public Figure BuildTriangle(Point rightBottomPoint)
    {
        var edgeLength = Random.Shared.Next(20, 50);
        
        Point spawnPoint = GetSpawnCoordinate(rightBottomPoint);
        var (dx, dy) = GetDxAndDy();
        
        var triangle = new Triangle(spawnPoint, dx, dy, edgeLength);
        
        return triangle;
    }
    
    private Point GetSpawnCoordinate(Point rightBottomPoint) => SpawnRandomizer.SpawnRandomizer.CreateBasedOnBottomRightPoint(rightBottomPoint).GetRandomPoint(50, 50);

    private (int, int) GetDxAndDy()
    {
        var random = new Random();

        if (random.Next(1, 3) == 1)
        {
            return (random.Next(15, 25), random.Next(15, 25));
        }

        return (random.Next(-25, -15), random.Next(-25, -15));
    }
}