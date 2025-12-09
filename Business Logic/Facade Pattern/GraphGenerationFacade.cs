using AI_Graphs.SingletonPattern;
using AI_Graphs.Utils;
using Microsoft.Maui.Graphics;
using System.Numerics;

namespace AI_Graphs.FacadePattern;

public class GraphGenerationFacade : IGraphGenerationFacade
{
    private readonly DataCollection _dataCollection;
    private readonly Random _random;
    private readonly CollisionDetectionSubsystem _collisionDetector;
    private readonly ColorGenerationSubsystem _colorGenerator;

    private const int MaxGenerationAttempts = 1000;
    private const int MinimumDistanceMultiplier = 2;

    public GraphGenerationFacade()
    {
        _dataCollection = DataCollection.GetInstance();
        _random = new Random();
        _collisionDetector = new CollisionDetectionSubsystem();
        _colorGenerator = new ColorGenerationSubsystem(_random);
    }

    public List<MyPoint> GenerateValidPoints(int width, int height)
    {
        int numPoints = _dataCollection.Amount;
        int minDistance = (int)_dataCollection.Radius;

        List<MyPoint> points = [];
        int attempts = 0;

        while (points.Count < numPoints)
        {
            int x = _random.Next(minDistance * 2 + 1, width - minDistance * 2 - 1);
            int y = _random.Next(minDistance * 2 + 1, height - minDistance * 2 - 1);

            MyPoint candidatePoint = new(x, y);

            if (_collisionDetector.IsValidPoint(candidatePoint, points, minDistance, width, height))
            {
                points.Add(candidatePoint);
                attempts = 0;
            }
            else
            {
                attempts++;

                if (attempts > MaxGenerationAttempts)
                {
                    points.Add(candidatePoint);
                    attempts = 0;
                }
            }
        }

        return points;
    }

    public List<Color> GenerateColorScheme()
    {
        int amount = _dataCollection.Amount;
        return _colorGenerator.GenerateRandomColors(amount);
    }

    public float GenerateRandomValue(double range, double min)
    {
        double sample = _random.NextDouble();
        double scaled = sample * range + min;
        return (float)scaled;
    }

    public bool CheckPointExists(List<Dictionary<int, Vector2>> collection, int key)
    {
        foreach (var item in collection)
        {
            if (item.ContainsKey(key))
            {
                return true;
            }
        }
        return false;
    }
}
