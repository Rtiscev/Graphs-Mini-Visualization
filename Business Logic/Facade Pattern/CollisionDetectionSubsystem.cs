using AI_Graphs.Utils;

namespace AI_Graphs.FacadePattern;

internal class CollisionDetectionSubsystem
{
    private const int RadiusMultiplier = 4;

    public bool IsValidPoint(MyPoint candidate, List<MyPoint> existingPoints, int minDistance, int width, int height)
    {
        if (IsOutOfBounds(candidate, minDistance, width, height))
        {
            return false;
        }

        foreach (var existingPoint in existingPoints)
        {
            if (HasCollision(candidate, existingPoint, minDistance))
            {
                return false;
            }
        }

        return true;
    }

    private bool IsOutOfBounds(MyPoint point, int minDistance, int width, int height)
    {
        return point.X + minDistance * 2 >= width ||
               point.Y + minDistance * 2 >= height ||
               point.X - minDistance * 2 <= 0 ||
               point.Y - minDistance * 2 <= 0;
    }

    private bool HasCollision(MyPoint point1, MyPoint point2, int minDistance)
    {
        float distance = (float)Math.Sqrt(
            Math.Pow(point2.X - point1.X, 2) +
            Math.Pow(point2.Y - point1.Y, 2)
        );

        return distance <= RadiusMultiplier * minDistance + RadiusMultiplier * minDistance;
    }
}
